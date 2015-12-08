#I "../packages/FSharp.Charting.0.90.13"
#r "..\\packages\\SQLProvider.0.0.9-alpha\\lib\\net40\\FSharp.Data.SqlProvider.dll"
#r "System"
#r "System.Core"
#r "System.Numerics"
#load "FSharp.Charting.fsx"

open FSharp.Charting
open System
open System.Collections.Generic
open System.Linq
open System.IO
open System.IO.Compression
open FSharp.Data.Sql

// create a type alias with the connection string and database vendor settings
type sql = SqlDataProvider< 
              ConnectionString = @"Data Source=C:\Users\t_mil\Documents\Visual Studio 2015\Projects\S3TestStuff\S3TestStuff\bin\Debug\new.sqlite ;Version=3",
              DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
              ResolutionPath = @"C:\Users\t_mil\Documents\Visual Studio 2015\Projects\Sqlite",
              IndividualsAmount = 1000,
              UseOptionTypes = true >

let ctx = sql.GetDataContext()

let firstVerb = (query {
      for t in ctx.``[main].[Entry]`` do
           where (t.verb =% ("GET%"))
           select (t.key)
           take 1
} |> Array.ofSeq ).[0]