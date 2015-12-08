module Program

open System
open System.Collections.Generic
open System.Linq
open System.IO
open System.IO.Compression
open System.Threading.Tasks
open Entry
open Database
open DownloadAmazon 

[<EntryPoint>]
let main args = 
    let currentMarker = ""
    Amazon.Util.ProfileManager.RegisterProfile("name", "key","key2")
    let markers = downloadAndStoreLogs(currentMarker) ("bucket") ("prefix") |> Async.RunSynchronously
    markers
        |> List.iter (printfn "%A")


    Console.ReadLine() |> ignore
    0