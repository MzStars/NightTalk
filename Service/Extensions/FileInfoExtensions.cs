using AutoMapper;
using Data;
using Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public static class FileInfoExtensions
    {
        public static FileInfo ToDbModel(this FileInfoDomain model)
        {
            return Mapper.Map<FileInfo>(model);
        }

        public static FileInfoDomain ToDomainModel(this FileInfo model)
        {
            return Mapper.Map<FileInfoDomain>(model);
        }
    }
}
