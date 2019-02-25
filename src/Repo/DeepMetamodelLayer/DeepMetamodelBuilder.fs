﻿(* Copyright 2019 Yurii Litvinov
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

namespace Repo.DeepMetamodelLayer

open Repo.DataLayer
open Repo.CoreModel

/// Initializes repository with Deep metamodel.
/// Deep metamodel is based on Core model and introduces notions of potency and linguistic/ontological instantiation.
type DeepMetamodelBuilder() =
   interface IModelBuilder with
       member this.Build(repo: IDataRepository): unit =
           let creator = CoreSemanticsModelCreator "DeepMetamodel"

           ()