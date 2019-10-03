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

namespace Repo.BasicMetamodel.DataObjects.Tests

open Repo.BasicMetamodel.DataObjects
open Repo.BasicMetamodel

open NUnit.Framework
open FsUnitTyped

[<TestFixture>]
type BasicMetamodelRepoTests() =

    [<SetUp>]
    member this.Setup () =
        ()

    [<Test>]
    member this.NameTest () =
        let node = BasicMetamodelNode("node") :> IBasicMetamodelNode
        node.Name |> shouldEqual "node"
        node.Name <- "newName"
        node.Name |> shouldEqual "newName"
        ()