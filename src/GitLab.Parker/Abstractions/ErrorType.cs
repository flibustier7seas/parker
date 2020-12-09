namespace GitLab.Parker.Abstractions
{
    public enum ErrorType
    {
        EnvironmentExistsAlready,
        EnvironmentDoesNotExist,
        EnvironmentTaken,
        EnvironmentTakenBySelf,
        EnvironmentProhibited,
        Forbidden
    }
}