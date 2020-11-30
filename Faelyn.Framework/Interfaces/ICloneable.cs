namespace Faelyn.Framework.Interfaces
{
    public interface ICloneable<out TClone>
    {
        TClone DeepClone();
    }
}