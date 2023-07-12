using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PefectMoney.Shared.Utility;

namespace PefectMoney.Shared.Utility
{
    public abstract class Enumeration<IdType,NameType> : IComparable
    {
        
        public NameType Name { get; private set; }

        public IdType Id { get; private set; }

        protected Enumeration(IdType id, NameType name) => (Id, Name) = (id, name);

        public override string ToString() => Name.ToString();

        public static IEnumerable<T> GetAll<T>() where T : Enumeration<IdType, NameType> =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null))
                     .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration<IdType, NameType> otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other)
        {
            if (other is not Enumeration<IdType, NameType> otherEnumeration)
            {
                throw new ArgumentException("Object is not of the correct type.");
            }

          
            return Convert.ToInt32(Id.Equals(otherEnumeration.Id));
        }

        public static  T FindById<T>(IdType id) where T : Enumeration<IdType, NameType> =>

            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                     .Select(f => f.GetValue(null) )
                     .Cast<T>()
                     .FirstOrDefault(x => x.Equals(id!))!;   
            
        // Other utility methods ...
    }
}
