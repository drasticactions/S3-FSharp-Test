module Entry
open System
open SQLite.Net.Attributes

[< CLIMutable >]
type Entry = {
    [<PrimaryKey>] [<AutoIncrement>]
    id: int;
    [<Indexed>] [<MaxLength(70)>]
    key: string;
    time: DateTime;
    [<MaxLength(7)>]
    edgeLocation: string;
    bytes: int;
    [<MaxLength(25)>]
    ip: string;
    [<MaxLength(5)>]
    verb: string;
    [<MaxLength(80)>]
    host: string;
    [<MaxLength(100)>]
    file: string;
    status: int;
    [<MaxLength(255)>]
    referer: string;
    [<MaxLength(255)>]
    userAgent: string;
}