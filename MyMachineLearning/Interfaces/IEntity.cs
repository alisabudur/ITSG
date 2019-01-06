namespace MyMachineLearning.Interfaces
{
    public interface IEntity
    {
        double[] ToInputModel();
        double[] ToOutputModel();
    }
}
