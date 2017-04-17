using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebPage.Models
{


    public class Meta
    {
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total_count { get; set; }
    }

    public class Alias
    {
        public string alias { get; set; }
        public string conference { get; set; }
        public string creation_time { get; set; }
        public string description { get; set; }
        public Nullable<int> id { get; set; }
    }

    public class IvrTheme
    {
        public Nullable<int> id { get; set; }
        public string name { get; set; }
        public string uuid { get; set; }
    }

    public class VMR
    {
        public List<Alias> aliases { get; set; }
        public bool allow_guests { get; set; }
        public List<object> automatic_participants { get; set; }
        public string call_type { get; set; }
        public string creation_time { get; set; }
        public string description { get; set; }
        public bool enable_overlay_text { get; set; }
        public bool force_presenter_into_main { get; set; }
        public string guest_pin { get; set; }
        public object guest_view { get; set; }
        public string host_view { get; set; }
        public Nullable<int> id { get; set; }
        public IvrTheme ivr_theme { get; set; }
        public string match_string { get; set; }
        public object max_callrate_in { get; set; }
        public object max_callrate_out { get; set; }
        public object mssip_proxy { get; set; }
        public bool mute_all_guests { get; set; }
        public string name { get; set; }
        public object participant_limit { get; set; }
        public string pin { get; set; }
        public string primary_owner_email_address { get; set; }
        public string replace_string { get; set; }
        public string resource_uri { get; set; }
        public string service_type { get; set; }
        public string sync_tag { get; set; }
        public object system_location { get; set; }
        public string tag { get; set; }
    }

    public class RootObjectVMR
    {
        public Meta meta { get; set; }
        public List<VMR> objects { get; set; }
    }

    public class RootObjectAlias
    {
        public Meta meta { get; set; }
        public List<Alias> objects { get; set; }
    }

    //This is for reading JSON dump from file instead of using the API while testing
    public class RootObjectFile
    {
        public object mssip_proxy { get; set; }
        public bool allow_guests { get; set; }
        public string creation_time { get; set; }
        public string host_view { get; set; }
        public object max_callrate_out { get; set; }
        public string tag { get; set; }
        public int id { get; set; }
        public List<Alias> aliases { get; set; }
        public string pin { get; set; }
        public bool enable_overlay_text { get; set; }
        public string sync_tag { get; set; }
        public string call_type { get; set; }
        public string replace_string { get; set; }
        public string service_type { get; set; }
        public bool mute_all_guests { get; set; }
        public object max_callrate_in { get; set; }
        public string description { get; set; }
        public object participant_limit { get; set; }
        public object guest_view { get; set; }
        public string guest_pin { get; set; }
        public object system_location { get; set; }
        public IvrTheme ivr_theme { get; set; }
        public string name { get; set; }
        public string primary_owner_email_address { get; set; }
        public List<object> automatic_participants { get; set; }
        public string match_string { get; set; }
        public bool force_presenter_into_main { get; set; }
        public string resource_uri { get; set; }
    }

}
