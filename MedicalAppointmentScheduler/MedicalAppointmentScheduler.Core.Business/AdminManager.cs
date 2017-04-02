using MedicalAppointmentScheduler.Core.Data;

using System;

using System.Collections.Generic;

using System.Data.Entity;

using System.Linq;

using System.Text;

using System.Threading.Tasks;



namespace MedicalAppointmentScheduler.Core.Business

{

    public class AdminManager

    {

        private MedicalSchedulerDBEntities dbContext;



        public AdminManager(MedicalSchedulerDBEntities _dbContext)

        {

            this.dbContext = _dbContext;

        }



        /// <summary>

        /// This method will delete the user 

        /// </summary>

        /// <param name="userId"></param>

        public void DeleteUser(int userId)

        {

            try

            {

                //Delete login entry

                UserLogin userLogin = dbContext.UserLogins.SingleOrDefault(u => u.UserID == userId);

                if (userLogin != null)

                {

                    dbContext.UserLogins.Remove(userLogin);

                }



                //delete user address

                UserAddress userAddress = dbContext.UserAddresses.SingleOrDefault(u => u.UserID == userId);

                if (userAddress != null)

                {

                    dbContext.UserAddresses.Remove(userAddress);

                }



                //delete user details

                UserDetails userDetails = dbContext.UserDetails.Find(userId);

                dbContext.UserDetails.Remove(userDetails);



                dbContext.SaveChanges();

            }

            catch (Exception)

            {

            }

        }



        /// <summary>

        /// This method update the user details 

        /// </summary>

        /// <param name="userDetails"></param>

        public void EditUser(UserDetails userDetails)

        {

            dbContext.Entry(userDetails).State = EntityState.Modified;

            dbContext.SaveChanges();

        }





        /// <summary>

        /// This is use to Create new user

        /// </summary>

        /// <param name="userDetails"></param>

        public void CreateUser(UserDetails userDetails)

        {

            dbContext.UserDetails.Add(userDetails);

            dbContext.SaveChanges();

        }



        /**

         * Used to retrieve data.  For testing purposes only.

         */

        public int getUserDetails(int id)

        {

            var roleId = dbContext.UserDetails.Where(o => o.ID.Equals(id)).Select(u => u.ID).SingleOrDefault();

            return roleId;

        }

    }

}
