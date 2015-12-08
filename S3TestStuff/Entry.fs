module Entry
open System
open SQLite.Net.Attributes

[< CLIMutable >]
type Entry = {
    [<PrimaryKey>] [<AutoIncrement>]
    id: int;
    key: string;
    time: DateTime;
    edgeLocation: string;
    bytes: int;
    ip: string;
    verb: string;    
    host: string;
    file: string;
    status: int;
    referer: string;
    userAgent: string;
}