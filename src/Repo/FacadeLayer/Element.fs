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

/// Repository with wrappers for elements (nodes or edges). Contains already created wrappers and creates new wrappers
/// when needed.
type IElementRepository =
    abstract GetElement: element: DataLayer.IDataElement -> IElement
    abstract DeleteElement: element: DataLayer.IDataElement -> unit

/// Implementation of an element wrapper.
and [<AbstractClass>] Element
    (
        infrastructureSemantic: InfrastructureMetamodel.InfrastructureSemantic
        , element: DataLayer.IDataElement
        , repository: IElementRepository
        , attributeRepository: AttributeRepository
    ) =

    let findMetatype (element : DataLayer.IDataElement) =
        failwith "Not implemented"
        //if infrastructureSemantic.Metamodel.IsNode element then
        //    Metatype.Node
        //elif infrastructureSemantic.Metamodel.IsEdge element then
        //    Metatype.Edge
        //else
        //    raise (InvalidSemanticOperationException
        //        "Trying to get a metatype of an element that is not instance of the Element. Model is malformed.")

    /// Returns corresponding element from data repository.
    member this.UnderlyingElement = element

    interface IElement with
        member this.Name
            with get (): string =
                failwith "Not implemented"
                //match element with
                //| :? DataLayer.IDataNode as n-> n.Name
                //| _ ->
                //    if infrastructureSemantic.Element.HasAttribute element "name" then
                //        infrastructureSemantic.Element.AttributeValue element "name"
                //    else
                //        ""

            and set (v: string): unit =
                failwith "Not implemented"
                //match element with
                //| :? DataLayer.IDataNode as n-> n.Name <- v
                //| _ ->
                //    if infrastructureSemantic.Element.HasAttribute element "name" then
                //        infrastructureSemantic.Element.SetAttributeValue element "name" v
                //    else
                //        raise (Repo.InvalidSemanticOperationException "Trying to set a name to an element which does not have one")

        member this.Attributes = 
            Seq.empty
            //infrastructureSemantic.Element.Attributes element |> Seq.map attributeRepository.GetAttribute

        member this.LinguisticType =
            repository.GetElement element.OntologicalType

        member this.OntologicalType =
            // TODO: Properly implement Ontological Type.
            repository.GetElement element.OntologicalType

        member this.IsAbstract =
            failwith "Not implemented"
            //if infrastructureSemantic.Element.InfrastructureMetamodel.IsElement element then
            //    if infrastructureSemantic.Element.InfrastructureMetamodel.IsGeneralization element then
            //        true
            //    else
            //        match infrastructureSemantic.Element.AttributeValue element "isAbstract" with
            //        | "true" -> true
            //        | "false" -> false
            //        | _ -> failwith "Incorrect isAbstract attribute value"
            //else
            //    true

        member this.Shape = 
            failwith "Not implemented"
            // infrastructureSemantic.Element.AttributeValue element "shape"

        member this.Metatype = findMetatype element

        member this.InstanceMetatype =
            failwith "Not implemented"
            //match infrastructureSemantic.Element.AttributeValue element "instanceMetatype" with
            //| "Metatype.Node" -> Metatype.Node
            //| "Metatype.Edge" -> Metatype.Edge
            //| _ -> failwith "Incorrect instanceMetatype attribute value"

        member this.AddAttribute (name, kind, defaultValue) =
            failwith "Not implemented"
            //infrastructureSemantic.Element.AddAttribute element name ("AttributeKind." + kind.ToString()) defaultValue
            //()
