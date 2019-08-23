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

namespace Repo

/// Factory that creates pre-configured repository.
[<AbstractClass; Sealed>]
type RepoFactory =
    /// Method that returns initialized repository.
    static member Create() = 
        let data = ((RepoFactory.CreateEmpty (): IRepo) :?> FacadeLayer.Repo).UnderlyingRepo
        let add (creator: DataLayer.IModelCreator) =
            creator.CreateIn data

        Metamodels.RobotsMetamodelCreator() |> add
        Metamodels.RobotsTestModelCreator() |> add
        //Metamodels.AirSimMetamodelBuilder() |> add
        //Metamodels.AirSimModelBuilder() |> add
        //Metamodels.FeatureMetamodelBuilder() |> add
        //Metamodels.FeatureTestModelBuilder() |> add

        new FacadeLayer.Repo(data) :> IRepo

    /// Method that returns a new repository populated from a save file.
    static member Load fileName =
        let data = new DataLayer.DataRepo() :> DataLayer.IDataRepository
        Serializer.Deserializer.load fileName data
        new FacadeLayer.Repo(data) :> IRepo

    /// Method that returns repository with infrastructure metamodel only.
    static member CreateEmpty () =
        let data = new DataLayer.DataRepo() :> DataLayer.IDataRepository
        let add (creator: DataLayer.IModelCreator) =
            creator.CreateIn data

        CoreMetamodel.CoreMetamodelCreator() |> add
        AttributeMetamodel.AttributeMetamodelCreator() |> add
        LanguageMetamodel.LanguageMetamodelCreator() |> add
        InfrastructureMetamodel.InfrastructureMetametamodelCreator() |> add

        new FacadeLayer.Repo(data) :> IRepo
