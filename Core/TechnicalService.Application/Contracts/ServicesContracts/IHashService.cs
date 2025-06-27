namespace TechnicalService.Application.Contracts.ServicesContracts
{
    public interface IHashService
    {
        // Password Hashing
        (string Hash, string Salt) HashItem(string Item);
        bool VerifyItem(string requestItem, string hashedItem, string saltItem);
        string GeneratePassword();
    }
}
