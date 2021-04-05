namespace Cms.Core.Rights
{
    public struct UserRights
    {
        public ulong R1;
        public ulong R2;
        public ulong R3;
        public ulong R4;

        public UserRights(ulong r1, ulong r2, ulong r3, ulong r4)
        {
            R1 = r1;
            R2 = r2;
            R3 = r3;
            R4 = r4;
        }

        public bool HashRight(UserRights ur)
        {
            return (this.R1 & ur.R1) == ur.R1 &&
                   (this.R2 & ur.R2) == ur.R2 &&
                   (this.R3 & ur.R3) == ur.R3 &&
                   (this.R4 & ur.R4) == ur.R4;
        }

        public override bool Equals(object obj)
        {
            if (obj is not UserRights)
            {
                return false;
            }

            var ur = (UserRights)obj;

            return this == ur;
        }

        private string ToBinarySring(ulong ul)
        {
            var s = "";
            while (ul > 0)
            {
                s = ((ul & 1) > 0 ? "1" : "0") + s;
                ul = ul >> 1;
            }

            return s.PadLeft(64, '0');
        }

        public override string ToString()
        {
            return $"{R1:X16}-{R2:X16}-{R3:X16}-{R4:X16}";
        }

        public static UserRights operator &(UserRights left, UserRights right)
        {
            return new UserRights(left.R1 & right.R1, left.R2 & right.R2, left.R3 & right.R3, left.R4 & right.R4);
        }

        public static UserRights operator |(UserRights left, UserRights right)
        {
            return new UserRights(left.R1 | right.R1, left.R2 | right.R2, left.R3 | right.R3, left.R4 | right.R4);
        }

        public static bool operator ==(UserRights left, UserRights right)
        {
            return
                left.R1 == right.R1 &&
                left.R2 == right.R2 &&
                left.R3 == right.R3 &&
                left.R4 == right.R4;
        }

        public static bool operator !=(UserRights left, UserRights right)
        {
            return !(left == right);
        }

        public static readonly UserRights MinValue = new UserRights(0, 0, 0, 0);
        public static readonly UserRights MaxValue = new UserRights(ulong.MaxValue, ulong.MaxValue, ulong.MaxValue, ulong.MaxValue);
    }
}
