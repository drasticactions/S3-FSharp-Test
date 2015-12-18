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
open Microsoft.FSharp.Linq
open System.IO
open System.IO.Compression
open FSharp.Data.Sql


// create a type alias with the connection string and database vendor settings
type sql = SqlDataProvider< 
              ConnectionString = @"Data Source=C:\Users\t_mil\Documents\Other-Apps\S3-FSharp-Test\S3TestStuff\bin\Debug\new.sqlite ;Version=3",
              DatabaseVendor = Common.DatabaseProviderTypes.SQLITE,
              ResolutionPath = @"C:\Users\t_mil\Documents\Visual Studio 2015\Projects\Sqlite",
              IndividualsAmount = 1000,
              UseOptionTypes = true >

let ctx = sql.GetDataContext()

let grouping = ctx.``[main].[Entry]``
               |> Seq.groupBy (fun x -> x.file)

for result in grouping do
    let animal = fst result
    let count = ((snd result) |> Seq.toArray).[0]
    printfn "%s : %s" "t" (count.ToString())