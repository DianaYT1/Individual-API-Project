using Services.Models;
using Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IFileReader
    {
        public ICollection<OrganizationModel> ReadDirectoryData(string dir);
    }
}
