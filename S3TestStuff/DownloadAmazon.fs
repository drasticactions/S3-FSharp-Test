module DownloadAmazon
open Entry
open System.Threading.Tasks
open System
open System.Collections.Generic
open System.Linq
open System.IO
open System.IO.Compression
open Amazon;
open Amazon.S3;
open Amazon.S3.Model;

let parseLine (key : string) (line: string) = 
    let chunks = line.Split('\t')
    {
        id = 0; // Placeholder ID. Actual value inserted by SQLite.
        key = key
        time = DateTime.Parse(chunks.[0] + " " + chunks.[1]);
        edgeLocation = chunks.[2];
        bytes  = Convert.ToInt32(chunks.[3]);
        ip     = chunks.[4];
        verb   = chunks.[5];
        host   = chunks.[6];
        file   = chunks.[7];
        status = Convert.ToInt32(chunks.[8]);
        referer = chunks.[9];
        userAgent = chunks.[10]
    }

let lines (response: GetObjectResponse) = 
    seq {
        use stream = response.ResponseStream
        use gzip   = new GZipStream(stream, CompressionMode.Decompress)
        use reader = new StreamReader(gzip)
        while not reader.EndOfStream do
            yield reader.ReadLine()
    }

let parseLog (response: GetObjectResponse) = 
    lines response
        |> Seq.skip 3
        |> Seq.map (parseLine response.Key) 
    

let downloadLogMarkers (marker: string) (bucket: string) (prefix: string) =
    async {
        use client = new AmazonS3Client(RegionEndpoint.USEast1)
        let req = new ListObjectsRequest(BucketName = bucket, Prefix = prefix, Marker = marker)
        let! resp = client.ListObjectsAsync(req) |> Async.AwaitTask
        let rec loop (resp: ListObjectsResponse, markers) = async {
            do printfn "Downloading %A" resp.NextMarker
            if (resp.NextMarker <> null) then
                let m = resp.NextMarker              
                let req = new ListObjectsRequest(BucketName = bucket, Prefix = prefix, Marker = m)
                let! resp = client.ListObjectsAsync(req) |> Async.AwaitTask
                return! loop(resp, m :: markers)
            else
                return markers                 
        }
        
        return! loop(resp, List.empty)
    }

let downloadAndStoreLogs (marker: string) (bucket: string) (prefix: string) = 
    async {
        let db = Database.createDatabase "new.sqlite"
        use client = new AmazonS3Client(RegionEndpoint.USEast1)
        let req = new ListObjectsRequest(BucketName = bucket, Prefix = prefix, Marker = marker)
        let! resp = client.ListObjectsAsync(req) |> Async.AwaitTask
        let rec loop (resp: ListObjectsResponse, markers) = async {
            for s3obj in resp.S3Objects do
                let request = new GetObjectRequest(BucketName = "xamarin-logs", Key = s3obj.Key)
                Console.WriteLine(request.Key)
                let response = client.GetObject(request)
                db.InsertAllAsync(parseLog(response)) |> Async.AwaitTask

            if (resp.NextMarker <> null) then
                let m = resp.NextMarker              
                let req = new ListObjectsRequest(BucketName = "xamarin-logs", Prefix = "xamarin-download-cf", Marker = m)
                let! resp = client.ListObjectsAsync(req) |> Async.AwaitTask
                return! loop(resp, m :: markers)
            else
                return markers                 
        }
        
        return! loop(resp, List.empty)
    }

//
//        let limit = false
//        let count = 0
//        let markers = []
//        let marker = ""
//        while not limit do
//        if not (marker.Equals("")) then
//            listObjectRequest.Marker = marker        
//
//        async {
//            let! objectListing = client.ListObjectsAsync(request = listObjectRequest)
//        }        
//        