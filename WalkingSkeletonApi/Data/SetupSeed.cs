﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalkingSkeletonApi.Data.Repositories.Database;

namespace WalkingSkeletonApi.Data
{
    public class SetupSeed
    {
        // check if the database already exist else create
        // check if the table already exist else create
        // check if table is not already seeded else seed it

        public static async Task SeedMe(IADOOperations adooperations)
        {

            string stmt1 = @"
                        Create Table AppUser(
                            id nvarchar (255) PRIMARY KEY,
                            lastName nvarchar(10) NOT NULL,
                            firstName nvarchar(10) NOT NULL,
                            email nvarchar(255) NOT NULL,
                            passwordHash nvarchar(255) NOT NULL,
                            passwordSalt nvarchar(255) NOT NULL
                        );

                        Create Table Roles(
                            id nvarchar (255) PRIMARY KEY,
                            RoleName nvarchar(10) NOT NULL
                        );";

            string stmt2 = @"
                        INSERT INTO AppUser (id, lastName, firstName, email, passwordHash, passwordSalt)
                        VALUES('1', 'John', 'Doe', 'john@doe.com', '12345', '12345');

                        
                        INSERT INTO Roles (id, RoleName)
                        VALUES('1', 'Admin'), ('2', 'Regular');";

            try
            {
                await adooperations.ExecuteForTransactionQuery(stmt1, stmt2);
            }catch(Exception ex)
            {
                // log to file
                Console.WriteLine(ex.Message);
            }
        }

    }
}
