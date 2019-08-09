﻿(* Copyright 2017 Yurii Litvinov
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

namespace Repo.FacadeLayer

open Repo
open Repo.Serializer

/// Wrapper around repository from data layer. Maintains a repository of models and creates new models if needed.
type Repo(repo: DataLayer.IDataRepository) =
    let infrastructure = InfrastructureMetamodel.InfrastructureSemantic(repo)

    let attributeRepository = AttributeRepository(repo)
    let elementRepository = ElementRepository(infrastructure, attributeRepository, repo)
    let modelRepository = ModelRepository(infrastructure, elementRepository)

    let unwrap (model: IModel) = (model :?> Model).UnderlyingModel

    /// Returns wrapped repository from a data layer.
    member this.UnderlyingRepo = repo

    interface IRepo with
        member this.Models
            with get () =
                repo.Models |> Seq.map modelRepository.GetModel

        member this.Model name =
            Helpers.getExactlyOne (this :> IRepo).Models
                (fun m -> m.Name = name)
                (fun () -> ModelNotFoundException name)
                (fun () -> MultipleModelsException name)

        member this.CreateModel (name: string, metamodel: IModel): IModel =
            failwith "Not implemented"
            //let underlyingModel = repo.CreateModel (name, unwrap metamodel)
            //modelRepository.GetModel underlyingModel

        member this.CreateModel (name, metamodelName) =
            let metamodel = (this :> IRepo).Model metamodelName
            (this :> IRepo).CreateModel (name, metamodel)

        member this.DeleteModel model =
            repo.DeleteModel (unwrap model)
            // TODO: Remove all elements from this model too.
            modelRepository.DeleteModel model

        member this.Save fileName =
            Serializer.save fileName repo
