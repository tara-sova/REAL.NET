(* Copyright 2017-2018 REAL.NET group
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. *)

namespace Repo.Metametamodels

open Repo.DataLayer
open Repo.CoreSemanticLayer
open Repo.InfrastructureSemanticLayer

/// Initializes repository with test model conforming to AirSim Metamodel, actual program that can be written by end-user.
type AppGenModelBuilder() =
    interface IModelBuilder with
        member this.Build(repo: IRepo): unit =
            let infrastructure = InfrastructureSemantic(repo)
            let metamodel = Repo.findModel repo "AppGenMetamodel"
            let infrastructureMetamodel = infrastructure.Metamodel.Model

            let metamodelAbstractNode = Model.findNode metamodel "AbstractNode"
            let metamodelMainFeatureModelNode = Model.findNode metamodel "MainFeatureModelNode"

            let link = Model.findAssociationWithSource metamodelAbstractNode "target"

            let model = repo.CreateModel("AppGenModel", metamodel)
            let mainFeatureModelNode = infrastructure.Instantiate model metamodelMainFeatureModelNode
            let find name = Model.findNode infrastructureMetamodel name

            // The same as in the metamodel but with functions
            let (~+) (name, shape, isAbstract) =
                let node = infrastructure.Instantiate metamodel (find "Node") :?> INode
                node.Name <- name
                infrastructure.Element.SetAttributeValue node "shape" shape
                infrastructure.Element.SetAttributeValue node "isAbstract" (if isAbstract then "true" else "false")
                infrastructure.Element.SetAttributeValue node "instanceMetatype" "Metatype.Node"

                node

            let (-->) (src: IElement) dst =
                let aLink = infrastructure.Instantiate model link :?> IAssociation
                aLink.Source <- Some src
                aLink.Target <- Some dst
                dst

            mainFeatureModelNode |> ignore

            ()
