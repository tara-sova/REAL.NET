﻿(* Copyright 2019 REAL.NET group
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

namespace Repo.AttributeMetamodel.Elements.Tests

open Repo
open Repo.AttributeMetamodel
open Repo.AttributeMetamodel.Details.Elements

open NUnit.Framework
open FsUnitTyped

[<TestFixture>]
type AttributeElementTests() =

    let mutable repo = (AttributeMetamodelRepoFactory.Create () :?> AttributeRepository).UnderlyingRepo
    let mutable factory = AttributeFactory(repo)
    let mutable pool = AttributePool(factory)
    let mutable model = repo.InstantiateCoreMetamodel "TestModel"

    let (~+) name = 
        let unwrappedNode = model.CreateNode name
        pool.Wrap (unwrappedNode)

    let (--->) (node1: IAttributeElement) (node2: IAttributeElement) =
        let unwrappedNode1 = (node1 :?> AttributeElement).UnderlyingElement
        let unwrappedNode2 = (node2 :?> AttributeElement).UnderlyingElement
        let unwrappedAssociation = model.CreateAssociation unwrappedNode1 unwrappedNode2 "testEdge"
        pool.Wrap unwrappedAssociation :?> IAttributeAssociation

    let (--|>) (node1: IAttributeElement) (node2: IAttributeElement) =
        let unwrappedNode1 = (node1 :?> AttributeElement).UnderlyingElement
        let unwrappedNode2 = (node2 :?> AttributeElement).UnderlyingElement
        let unwrappedGeneralization = model.CreateGeneralization unwrappedNode1 unwrappedNode2
        pool.Wrap unwrappedGeneralization |> ignore

    [<SetUp>]
    member this.Setup () =
        repo <- (AttributeMetamodelRepoFactory.Create () :?> AttributeRepository).UnderlyingRepo
        factory <- AttributeFactory(repo)
        pool <- AttributePool(factory)
        model <- repo.InstantiateCoreMetamodel "TestModel"

    [<Test>]
    member this.OutgoingAssociationsTest () =
        let node1 = +"Node1"
        let node2 = +"Node2"
        let edge = node1 ---> node2
        node1.OutgoingAssociations |> shouldContain edge
        node1.OutgoingAssociations |> shouldHaveLength 1
        ()

    [<Test>]
    member this.AttributesTest () =
        let node = +"Node"
        let ``type`` = +"Type"
        node.AddAttribute "attribute" ``type``

        node.Attributes |> Seq.filter (fun a -> a.Name = "attribute") |> shouldHaveLength 1

    [<Test>]
    [<Ignore("Not implemented")>]
    member this.AddingTwoAttributesWithTheSameNameAreNotAllowedTest () =
        let node = +"Node"
        let ``type`` = +"Type"
        
        node.AddAttribute "attribute" ``type``
        (fun () -> node.AddAttribute "attribute" ``type``) |> shouldFail<AmbiguousAttributesException>

    [<Test>]
    [<Ignore("Not implemented")>]
    member this.AttributesRespectGeneralizationTest () =
        let parent = +"Parent"
        let ``type`` = +"Type"
            
        parent.AddAttribute "attribute" ``type``

        let child = +"Child"
        child --|> parent

        child.Attributes |> Seq.filter (fun a -> a.Name = "attribute") |> shouldHaveLength 1