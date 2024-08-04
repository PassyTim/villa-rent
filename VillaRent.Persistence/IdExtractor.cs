using System.Linq.Expressions;
using System.Reflection;
using VillaRent.Domain.Models;

namespace VillaRent.Persistence;

public static class IdExtractor
{
    public static int Extract(Expression<Func<Villa, bool>> expression)
    {
        var binaryExpression = (BinaryExpression)expression.Body;
        
        var right = binaryExpression.Right;
        var intKey = 0;
        if (right is ConstantExpression constantExpression)
        {
            return intKey = (int)constantExpression.Value;
        }
        else if (right is MemberExpression memberExpression)
        {
            
            var constant = (ConstantExpression)memberExpression.Expression;
            var fieldInfo = (FieldInfo)memberExpression.Member;
            var capturedValue = fieldInfo.GetValue(constant.Value);
            return intKey = (int)capturedValue;
        }
        
        throw new InvalidOperationException();
    }
}