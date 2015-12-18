module Database

open System
open System.Linq
open System.Threading.Tasks
open SQLite.Net
open SQLite.Net.Async
open SQLite.Net.Platform.Generic

open Entry

type sqlite_master = { name: string }

let createDatabase (filename: string) (existingTables: bool) =
    let db = new SQLiteAsyncConnection (fun _ -> new SQLiteConnectionWithLock(new SQLitePlatformGeneric(), new SQLiteConnectionString(filename, false)))
    if not (existingTables) then 
        db.CreateTableAsync<Entry>().GetAwaiter().GetResult() |> ignore
    db

let currentEntries (filename: string) =
    let db = new SQLiteAsyncConnection (fun _ -> new SQLiteConnectionWithLock(new SQLitePlatformGeneric(), new SQLiteConnectionString(filename, false)))
    db.Table<Entry>().ToListAsync().GetAwaiter().GetResult()
