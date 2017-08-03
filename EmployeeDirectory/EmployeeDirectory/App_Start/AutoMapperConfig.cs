using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Employee.DomainObject;
using EmployeeDirectory.Models;
using Microsoft.Practices.Unity;
using NHibernate.Criterion;

namespace EmployeeDirectory.App_Start
{
    public class AutoMapperConfig
    {
        public static void ConfigureMapping()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PhoneNumberViewModel, PhoneNumber>();
                cfg.CreateMap<PhoneNumber, PhoneNumberViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            UnityConfig.Current.RegisterInstance(mapper, new ContainerControlledLifetimeManager());
        }
    }
}