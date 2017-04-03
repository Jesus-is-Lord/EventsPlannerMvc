using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsPlannerMvc.Models
{
    [MetadataType(typeof(EventMetaData))]
    public partial class Event
    {
        public bool HasIdeas
        {
            get
            {
                foreach (var m in this.Members)
                    if (m.Ideas.Count > 0)
                        return true;
                return false;
            }
        }
    }

    public class EventMetaData
    {
        [Required]
        [DisplayName("Event Date")]
        [DisplayFormat(DataFormatString = "{0:f}")] //format pattern f is Full date/time (short time), e.g. Sunday, August 11, 2008 3:32 PM
        public System.DateTime EventDate { get; set; }
    }

}