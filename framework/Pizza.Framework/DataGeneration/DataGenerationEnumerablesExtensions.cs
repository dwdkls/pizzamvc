using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pizza.Framework.DataGeneration
{
    public static class DataGenerationEnumerablesExtensions
    {
        private static readonly Random Rand = new Random();

        /// <summary>
        /// Sets values of some property using values drawn from specified collection. Applies to all items in source collection.
        /// </summary>
        public static IEnumerable<TItem> SetRandomValuesForAllItems<TItem, TProperty>(this IEnumerable<TItem> sourceItems,
            Expression<Func<TItem, TProperty>> propertyExpression, IList<TProperty> itemsToDrawn)
        {
            var property = GetPropertyToUpdate(propertyExpression);

            var itemsList = sourceItems as IList<TItem> ?? sourceItems.ToList();
            foreach (var item in itemsList)
            {
                var randomSubPropertyItem = itemsToDrawn[Rand.Next(0, itemsToDrawn.Count)];
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, randomSubPropertyItem);
            }

            return itemsList;
        }

        /// <summary>
        /// Sets values of some property using specified value. Applies to specified amount of random items in source collection.
        /// </summary>
        public static IEnumerable<TItem> SetFixedValueForSomeRandomItems<TItem, TProperty>(this IEnumerable<TItem> items,
          Expression<Func<TItem, TProperty>> propertyExpression, TProperty value, int count)
        {
            var itemsList = items as IList<TItem> ?? items.ToList();
            AssertCount(count, itemsList.Count);

            var randomized = itemsList.OrderBy(x => Guid.NewGuid()).Take(count);
            var property = GetPropertyToUpdate(propertyExpression);
            foreach (var item in randomized)
            {
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, value);
            }

            return itemsList;
        }

        /// <summary>
        /// Sets values of some property using specified generator function. Applies to specified amount of random items in source collection.
        /// </summary>
        public static IEnumerable<TItem> SetGeneratedValuesForSomeRandomItems<TItem, TProperty>(this IEnumerable<TItem> items,
          Expression<Func<TItem, TProperty>> propertyExpression, Func<TProperty> valueGenerator, int count)
        {
            var itemsList = items as IList<TItem> ?? items.ToList();
            AssertCount(count, itemsList.Count);

            var randomized = itemsList.OrderBy(x => Guid.NewGuid()).Take(count);
            var property = GetPropertyToUpdate(propertyExpression);

            foreach (var item in randomized)
            {
                var value = valueGenerator.Invoke();
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, value);
            }

            return itemsList;
        }

        /// <summary>
        /// Sets values of some property using one specified value. Applies for specified range of items in source collection. 
        /// </summary>
        public static IEnumerable<TItem> SetFixedValuesForRangeOfItems<TItem, TProperty>(this IEnumerable<TItem> sourceItems,
            Expression<Func<TItem, TProperty>> propertyExpression, TProperty value, int startIndex, int count)
        {
            int endIndex = startIndex + count;
            var itemsList = sourceItems as IList<TItem> ?? sourceItems.ToList();
            AssertIndices(startIndex, endIndex, itemsList.Count);

            var property = GetPropertyToUpdate(propertyExpression);
            for (int i = startIndex; i < endIndex; i++)
            {
                var targetItem = GetObjectContainingUpdatedProperty(itemsList[i], propertyExpression);
                property.SetValue(targetItem, value);
            }

            return itemsList;
        }

        /// <summary>
        /// Sets values of some property using specified generator function. Applies for specified range of items in source collection. 
        /// </summary>
        public static IEnumerable<TItem> SetGeneratedValuesForRangeOfItems<TItem, TProperty>(this IEnumerable<TItem> sourceItems,
            Expression<Func<TItem, TProperty>> propertyExpression, Func<TProperty> valueGenerator, int startIndex, int count)
        {
            int endIndex = startIndex + count;
            var itemsList = sourceItems as IList<TItem> ?? sourceItems.ToList();
            AssertIndices(startIndex, endIndex, itemsList.Count);

            var property = GetPropertyToUpdate(propertyExpression);
            for (int i = startIndex; i < endIndex; i++)
            {
                var value = valueGenerator.Invoke();
                var targetItem = GetObjectContainingUpdatedProperty(itemsList[i], propertyExpression);
                property.SetValue(targetItem, value);
            }

            return itemsList;
        }

        /// <summary>
        /// Sets values of some property using specified generator function. Applies to all items in source collection.
        /// </summary>
        public static IEnumerable<TItem> SetGeneratedValuesForAllItems<TItem, TProperty>(this IEnumerable<TItem> sourceItems,
            Expression<Func<TItem, TProperty>> propertyExpression, Func<TProperty> valueGenerator)
        {
            var property = GetPropertyToUpdate(propertyExpression);

            var itemsList = sourceItems as IList<TItem> ?? sourceItems.ToList();
            foreach (var item in itemsList)
            {
                var value = valueGenerator.Invoke();
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, value);
            }

            return itemsList;
        }

        /// <summary>
        /// Sets values of some property using specified value. Applies to all items in source collection.
        /// </summary>
        public static IEnumerable<TItem> SetFixedValueForAllItems<TItem, TProperty>(this IEnumerable<TItem> sourceItems,
            Expression<Func<TItem, TProperty>> propertyExpression, TProperty value)
        {
            var property = GetPropertyToUpdate(propertyExpression);

            var itemsList = sourceItems as IList<TItem> ?? sourceItems.ToList();
            foreach (var item in itemsList)
            {
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, value);
            }

            return itemsList;
        }

        /// <summary>
        /// Sets values of some property using possible values set - one value will be set for specifed amount of items and others will be drawn
        /// </summary>
        public static IEnumerable<TItem> SetFixedValueForSomeItemsAndOtherValuesForOtherItems<TItem, TProperty>(
            this IEnumerable<TItem> sourceItems,
            Expression<Func<TItem, TProperty>> propertyExpression, 
            TProperty fixedValue,
            int count,
            IList<TProperty> otherValues)
        {
            var property = GetPropertyToUpdate(propertyExpression);

            var itemsList = sourceItems.ToList();

            foreach (var item in itemsList)
            {
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, null);
            }
            
            var randomized = itemsList.OrderBy(x => Guid.NewGuid()).Take(count);
            foreach (var item in randomized)
            {
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, fixedValue);
            }

            var propFunc = propertyExpression.Compile();
            foreach (var item in itemsList.Where(x => propFunc(x) == null))
            {
                var randomSubPropertyItem = otherValues[Rand.Next(0, otherValues.Count)];
                var targetItem = GetObjectContainingUpdatedProperty(item, propertyExpression);
                property.SetValue(targetItem, randomSubPropertyItem);
            }

            return itemsList;
        }

        private static void AssertCount(int count, int listCount)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "count must be greater than 0");
            }
            if (count > listCount)
            {
                throw new ArgumentOutOfRangeException("count", "count must be less than items count");
            }
        }

        private static void AssertIndices(int startIndex, int endIndex, int count)
        {
            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater than 0");
            }
            if (endIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex", "endIndex must be greater than 0");
            }
            if (startIndex >= endIndex)
            {
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be less than endIndex");
            }
            if (endIndex >= count)
            {
                throw new ArgumentOutOfRangeException("endIndex", "endIndex must be less than items count");
            }
        }


        private static object GetObjectContainingUpdatedProperty<TItem, TProperty>(
            TItem item, Expression<Func<TItem, TProperty>> propertyExpression)
        {
            // TODO: try to simplify this

            var expression = (MemberExpression)propertyExpression.Body;

            // when expression looks like: x.PropertyToSet then return just item item (because item is x in expression)
            if (expression.Expression.NodeType == ExpressionType.Parameter)
            {
                return item;
            }

            // when expression looks like x.PropA.OtherProp.PropertyToSet then we have to find OtherProp.PropertyToSet
            if (expression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                expression = expression.Expression as MemberExpression;
            }

            object propertyParent = item;

            // when expression is x.OtherProp.PropertyToSet, then this memberExpression will be null
            var memberExpression = expression.Expression as MemberExpression;
            if (memberExpression != null)
            {
                var subparentex = (PropertyInfo)memberExpression.Member;
                var subparent = subparentex.GetValue(item, null);
                propertyParent = subparent;
            }

            var subprop = (PropertyInfo)expression.Member;
            var subitem = subprop.GetValue(propertyParent, null);

            return subitem;
        }

        private static PropertyInfo GetPropertyToUpdate<TItem, TProperty>(Expression<Func<TItem, TProperty>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var property = (PropertyInfo)expression.Member;
            return property;
        }
    }
}