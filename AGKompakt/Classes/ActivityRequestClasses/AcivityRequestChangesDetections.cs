using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGH_Kompakt.Classes.ActivityRequestClasses
{
    public class PropertyChange
    {
        public string PropertyName { get; set; } = string.Empty;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }

    public static class ActivityRequestComparer
    {
        public static List<PropertyChange> GetChanges(ActivityRequest original, ActivityRequest updated)
        {
            var changes = new List<PropertyChange>();
            if (original == null || updated == null) return changes;

            var props = typeof(ActivityRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && !p.GetIndexParameters().Any());

            foreach (var prop in props)
            {
                // ignore computed / NotMapped properties
                if (prop.GetCustomAttributes(inherit: true).Any(a => a.GetType().Name == "NotMappedAttribute"))
                    continue;

                var type = prop.PropertyType;
                // compare simple / scalar types only (string, numeric, DateTime, bool, nullable of same)
                if (IsSimpleType(type))
                {
                    var oldVal = prop.GetValue(original);
                    var newVal = prop.GetValue(updated);

                    if (!ObjectEquals(oldVal, newVal))
                    {
                        changes.Add(new PropertyChange
                        {
                            PropertyName = prop.Name,
                            OldValue = ToStringSafe(oldVal),
                            NewValue = ToStringSafe(newVal)
                        });
                    }
                }
            }

            // Special checks for collections / aggregates that are meaningful for AR
            // 1) ARVerguetungAdventages: sum of amounts
            decimal sumOld = original.ARVerguetungAdventages?.Sum(a => a?.ARVerguetungAdventageAmount ?? 0) ?? 0;
            decimal sumNew = updated.ARVerguetungAdventages?.Sum(a => a?.ARVerguetungAdventageAmount ?? 0) ?? 0;
            if (sumOld != sumNew)
            {
                changes.Add(new PropertyChange
                {
                    PropertyName = "ARVerguetungAdventagesSum",
                    OldValue = sumOld.ToString("0.##"),
                    NewValue = sumNew.ToString("0.##")
                });
            }

            // 2) collection counts for attachments / arbitration clients / data files / change histories
            CompareCollectionCount(original.ActivityRequestArbitrationClients, updated.ActivityRequestArbitrationClients, "ActivityRequestArbitrationClients", changes);
            CompareCollectionCount(original.ActivityRequestDataFiles, updated.ActivityRequestDataFiles, "ActivityRequestDataFiles", changes);
            CompareCollectionCount(original.ARVerguetungAdventages, updated.ARVerguetungAdventages, "ARVerguetungAdventages", changes);
            CompareCollectionCount(original.ActivityRequestChangeHistories, updated.ActivityRequestChangeHistories, "ActivityRequestChangeHistories", changes);

            return changes;
        }

        private static void CompareCollectionCount(IEnumerable? oldCol, IEnumerable? newCol, string name, IList<PropertyChange> changes)
        {
            int oldCount = CountOrZero(oldCol);
            int newCount = CountOrZero(newCol);
            if (oldCount != newCount)
            {
                changes.Add(new PropertyChange
                {
                    PropertyName = name + "Count",
                    OldValue = oldCount.ToString(),
                    NewValue = newCount.ToString()
                });
            }
        }

        private static int CountOrZero(IEnumerable? col)
        {
            if (col == null) return 0;
            if (col is ICollection c) return c.Count;
            var enumerator = col.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext()) count++;
            return count;
        }

        private static bool IsSimpleType(Type type)
        {
            Type? underlying = Nullable.GetUnderlyingType(type) ?? type;
            if (underlying.IsEnum) return true;
            if (underlying == typeof(string) ||
                underlying == typeof(decimal) ||
                underlying == typeof(DateTime) ||
                underlying == typeof(DateTimeOffset) ||
                underlying == typeof(Guid) ||
                underlying == typeof(TimeSpan))
                return true;
            return underlying.IsPrimitive || underlying.IsValueType;
        }

        private static bool ObjectEquals(object? a, object? b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a is IComparable && b is IComparable)
            {
                return a.Equals(b);
            }
            return a.Equals(b);
        }

        private static string? ToStringSafe(object? o)
        {
            if (o == null) return null;
            if (o is DateTime dt) return dt.ToString("s"); // sortable
            if (o is float f) return f.ToString("0.##");
            if (o is double d) return d.ToString("0.##");
            if (o is decimal m) return m.ToString("0.##");
            return o.ToString();
        }

        private static string GetCurrentUserName()
        {
            try
            {
                return UserManager?.RegistratedUser?.Fullname ?? Environment.UserName;
            }
            catch { return Environment.UserName; }
        }

        // Helper to allow optional UserManager resolution without hard dependency issues in unit tests
        private static dynamic? UserManager
        {
            get
            {
                // fallback: try to resolve the static UserManager used in the project (if present)
                var umType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a =>
                    {
                        try { return a.GetTypes(); } catch { return Type.EmptyTypes; }
                    })
                    .FirstOrDefault(t => t.Name == "UserManager");
                if (umType == null) return null;
                var prop = umType.GetProperty("RegistratedUser", BindingFlags.Public | BindingFlags.Static);
                if (prop == null) return null;
                var registrated = prop.GetValue(null);
                return registrated == null ? null : new { RegistratedUser = registrated };
            }
        }
    }
}
