using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCBlog.Enums
{
    ////Enum - The MVC framework sees the enum value as a simple primitive and uses the default string template. 
    //One solution to change the default framework behavior is to write a custom model metadata provider and implement GetMetadataForProperty 
    //to use a template with the name of "Enum" for such models.
    public enum Roles
    {
        Administrator,
        Moderator
    }
}
