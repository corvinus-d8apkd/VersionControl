﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnitTestExample.Controllers;
using Assert = NUnit.Framework.Assert;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        public Guid ID { get; private set; }

        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-corvinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {

            var accountController = new AccountController();
            var actualResult = accountController.ValidateEmail(email);
            Assert.AreEqual(expectedResult, actualResult);

        }
        public bool ValidatePassword(string password)
        {
            var kisbetus = new Regex(@"[a-z]+");
            var nagybetus = new Regex(@"[A-Z]+");
            var szam = new Regex(@"[0-9]+");
            var nyolchosszu = new Regex(@".{8,}");
            return kisbetus.IsMatch(password) && nagybetus.IsMatch(password) && szam.IsMatch(password) && nyolchosszu.IsMatch(password);
        }

        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Abcd1234567"),
        ]
        public void TestRegisterHappyPath(string email, string password)
        {
            ID = Guid.NewGuid();
            var accountController = new AccountController();
            
            var actualResult = accountController.Register(email, password);
            Assert.AreEqual(email, actualResult.Email);
            //Assert.AreEqual(password, actualResult.Password);
            Assert.AreNotEqual(Guid.Empty, actualResult.ID);
        }
        [
            Test,
            TestCase("irf@uni-corvinus", "Abcd1234"),
            TestCase("irf.uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "abcd1234"),
            TestCase("irf@uni-corvinus.hu", "ABCD1234"),
            TestCase("irf@uni-corvinus.hu", "abcdABCD"),
            TestCase("irf@uni-corvinus.hu", "Ab1234"),
        ]
        public void TestRegisterValidateException(string email, string password)
        {
            var accountController = new AccountController();
            try
            {
                var actualResult = accountController.Register(email, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ValidationException>(ex);
            }

        }
    }
    
}
