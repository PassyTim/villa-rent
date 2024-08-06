using System.Linq.Expressions;
using System.Reflection;

namespace VillaRent.Persistence;

public static class IdExtractor
{
    public static int Extract<T>(Expression<Func<T, bool>> expression)
    {
        var binaryExpression = (BinaryExpression)expression.Body;
        
        var right = binaryExpression.Right;
        if (right is ConstantExpression constantExpression)
        {
            return (int)constantExpression.Value;
        }
        else if (right is MemberExpression memberExpression)
        {
            
            var constant = (ConstantExpression)memberExpression.Expression;
            var fieldInfo = (FieldInfo)memberExpression.Member;
            var capturedValue = fieldInfo.GetValue(constant.Value);
            return (int)capturedValue;
        }
        
        throw new InvalidOperationException();
    }
}