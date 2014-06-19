#region Reegenerator:{Template:"NotifyProperty",Regen:"Never",Date:"2014-06-18T15:52:31.7312521+08:00"}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace Test {
    internal interface INotifier : System.ComponentModel.INotifyPropertyChanged {
        void Notify(string propertyName);
    }

    internal static class NotifyPropertyChanged_Gen_Extensions {
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


        public static void NotifyChanged<T>(this INotifier notifier, System.Linq.Expressions.Expression<Func<T, object>> memberExpr) {
            notifier.Notify(ExprToString(memberExpr));
        }

        public static void NotifyChanged<T>(this INotifier notifier, params System.Linq.Expressions.Expression<Func<T, object>>[] propExpressions) {
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
            System.Linq.Expressions.Expression currExpr = null;
            //when T2 is object, the expression will be wrapped in UnaryExpression of Convert{}
            var convertedToObject = memberExpr.Body as UnaryExpression;
            if (convertedToObject != null) {
                //unwrap
                currExpr = convertedToObject.Operand;
            }
            else {
                currExpr = memberExpr.Body;
            }
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
