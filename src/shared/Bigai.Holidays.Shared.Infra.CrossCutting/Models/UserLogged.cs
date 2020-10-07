using Bigai.Holidays.Shared.Infra.CrossCutting.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Models
{
    /// <summary>
    /// <see cref="UserLogged"/> implements a contract for the information of the logged user.
    /// </summary>
    public class UserLogged : IUserLogged
    {
        #region Métodos privados

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Guid _userId;

        #endregion

        #region Constructor

        public UserLogged(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _userId = Guid.Parse("8987EF64-B45A-4545-9D5B-EFE0EDEC6147");
        }

        #endregion

        #region Public Methods

        public Guid GetUserId()
        {
            return _userId;
        }

        public string GetUserIp()
        {
            string ipUsuario = "";

            System.Net.IPAddress ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            if (ip != null)
            {
                ipUsuario = ip.ToString() == "::1" ? "localhost" : ip.ToString();
            }
            return ipUsuario;
        }

        #endregion
    }
}
