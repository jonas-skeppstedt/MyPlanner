namespace MyPlanner.Domain.Common
{
    public interface IFailureBuildable<TSelf>
        where TSelf : Result
    {
        static abstract TSelf Failure(Error error);
    }
}
