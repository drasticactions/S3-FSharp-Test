using System;
using SQLite.Net.Attributes;

namespace EntryLibrary
{
    public class Entry
    {
        [PrimaryKey]
        [AutoIncrement]
        public int id { get; set; }

        public DateTime time { get; set; }
        public string edgeLocation { get; set; }

        public int bytes { get; set; }

        public string ip { get; set; }

        public string verb { get; set; }

        public string host { get; set; }

        public string file { get; set; }

        public int status { get; set; }

        public string referer { get; set; }

        public string userAgent { get; set; }
    }
}
