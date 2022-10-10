using AutoTestMVC.Models;
using AutoTestMVC.Repositories;

namespace AutoTestMVC.Services
{
    public class UsersService
    {
        private CookiesService _cookiesService;
        private UsersRepository _usersRepository;

        public UsersService()
        {
            _cookiesService = new CookiesService();
            _usersRepository = new UsersRepository();
        }

        public User? GetUserFromCookie(HttpContext context)
        {
            var userPhone = _cookiesService.GetUserPhoneFromCookie(context);

            if (userPhone != null)
            {
                var user = _usersRepository.GetUserByPhone(userPhone);
                if (user.Phone == userPhone)
                {
                    return user;
                }
            }

            return null;
        }
    }
}
