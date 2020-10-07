using System;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces
{
    /// <summary>
    /// <see cref="IUserLogged"/> represents a contract for the information of the logged user.
    /// </summary>
    public interface IUserLogged
    {
        /// <summary>
        /// Determines the IP address of the user who is logged in.
        /// </summary>
        /// <returns>The IP address of the user who is logged in.</returns>
        string GetUserIp();

        /// <summary>
        /// Determines the user Id of who is logged in.
        /// </summary>
        /// <returns>The user id of who is logged in.</returns>
        Guid GetUserId();

        /// 
        /// TODO: Deve ser removido com a implementação de login identity
        /// 
    }
}
