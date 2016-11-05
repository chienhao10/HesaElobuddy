using Automation.Enums;

namespace Automation.Controllers
{
    public static class RoleController
    {
        private static Role _myRole = Role.Unknown;
        public static Role MyRole
        {
            get
            {
                if(_myRole == Role.Unknown)
                {
                    _myRole = GetMyRole();
                }
                return _myRole;
            }
        }
        
        private static Role GetMyRole()
        {

            return Role.Unknown;
        }

        private static Role[] OtherTeamPlayersRole()
        {
            Role[] returnValue = new Role[4];

            return returnValue;
        }

    }
}