using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sale.Common.Helpers
{
   public  interface IFilesHelper
    {
        byte[] ReadFully(Stream input);

    }
}
