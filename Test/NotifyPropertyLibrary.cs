using System.ComponentModel;

#region  Reegenerator:{Template:"NotifyProperty",Date:"2014-06-23T20:08:07.2266019+08:00"}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Test {
    internal interface INotifier : INotifyPropertyChanged {
        void Notify(string propertyName);
    }

    internal static class NotifyPropertyLibrary {
        public static void NotifyChanged(this INotifier notifier, string propertyName) {
            notifier.Notify(propertyName);
        }





        public static bool SetPropertyAndNotify<T>(this INotifier notifier, ref T field, T newValue, string propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, newValue)) {
                return false;
            }
            field = newValue;
            notifier.NotifyChanged(propertyName);
            return true;
        }


        public static void NotifyChanged<T>(this INotifier notifier, Expression<Func<T, object>> memberExpr) {
            notifier.Notify(ExprToString(memberExpr));
        }

        public static void NotifyChanged<T>(this INotifier notifier, params Expression<Func<T, object>>[] propExpressions) {
            foreach (var p in propExpressions) {
                notifier.NotifyChanged(p);
            }
        }

        public static void NotifyChanged(this INotifier notifier, params string[] props) {
            foreach (var p in props) {
                notifier.NotifyChanged(p);
            }
        }

        #region ExprToString

        public static string[] ExprsToString<T>(params Expression<Func<T, object>>[] exprs) {

            var strings = (
                from x in exprs
                select ((LambdaExpression)x).ExprToString()).ToArray();
            return strings;
        }

        public static string ExprToString<T, T2>(this Expression<Func<T, T2>> expr) {
            return ((LambdaExpression)expr).ExprToString();
        }

        public static string ExprToString(this LambdaExpression memberExpr) {
            if (memberExpr == null) {
                return "";
            }
            //when T2 is object, the expression will be wrapped in UnaryExpression of Convert{}
            var convertedToObject = memberExpr.Body as UnaryExpression;
            var currExpr = convertedToObject != null ? convertedToObject.Operand : memberExpr.Body;
            switch (currExpr.NodeType) {
                case ExpressionType.MemberAccess:
                    var ex = (MemberExpression)currExpr;
                    return ex.Member.Name;
            }

            throw new Exception("Expression ToString() extension only processes MemberExpression");
        }

        #endregion
    }
}





#endregion
