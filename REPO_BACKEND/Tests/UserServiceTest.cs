//using backnc.Common;
//using backnc.Data.Interface;
//using backnc.Data.POCOEntities;
//using backnc.Interfaces;
//using backnc.Models;
//using backnc.Service;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//using Moq;
//using Xunit;

//namespace backnc.Tests
//{
//    [TestClass]
//	public class UserServiceTest
//	{
//		private Mock<IAppDbContext> _mockContext;
//		private Mock<IUserValidationService> _mockUserValidationService;
//		private Mock<ITokenService> _mockTokenService;
//		private Mock<IConfiguration> _mockConfiguration;		
//		private UserService _userService;

//		[TestInitialize]
//		public void Setup()
        
//        {
//			_mockContext = new Mock<IAppDbContext>();
//			_mockUserValidationService = new Mock<IUserValidationService>();
//			_mockTokenService = new Mock<ITokenService>();
//			_mockConfiguration = new Mock<IConfiguration>();

//			_userService = new UserService(
//				_mockContext.Object,
//				_mockConfiguration.Object,
//				_mockUserValidationService.Object,
//				_mockTokenService.Object
//			);



//		}

//		[TestMethod]
//		public async Task Authenticate_UserNotFound_ReturnsValidationError()
//		{
//			var loginUser = new LoginUser { UserName = "nonexistentUser", Password = "password123" };
//			_mockContext.Setup(ctx => ctx.Users)
//				.Returns(CreateDb)

//			var result = await _userService.Authenticate(loginUser);

//			Assert.IsFalse(result.IsSuccess);
//			Assert.AreEqual("Usuario no encontrado", result.message);
//		}

//		[TestMethod]
//		public async Task Authenticate_InvalidPassword_ReturnsValidationError()
//		{
//			var loginUser = new LoginUser { UserName = "existingUser", Password = "wrongpassword" };
//			var existingUser = new User { UserName = "existingUser", Password = PasswordHasher.VerifyPassword("correctpassword") };

//			_mockContext.Setup(ctx => ctx.Users)
//				.ReturnsDbSet(new List<User> { existingUser });

//			var result = await _userService.Authenticate(loginUser);

//			Assert.IsFalse(result.IsSuccess);
//			Assert.AreEqual("Credenciales incorrectas", result.message);
//		}

//		[TestMethod]
//		public async Task Authenticate_ValidCredentials_ReturnsToken()
//		{
//			var loginUser = new LoginUser { UserName = "existingUser", Password = "correctpassword" };
//			var existingUser = new User { UserName = "existingUser", Password = PasswordHasher.HashPassword("correctpassword") };
//			string expectedToken = "mockToken";

//			_mockContext.Setup(ctx => ctx.Users)
//				.ReturnsDbSet(new List<User> { existingUser });

//			_mockTokenService.Setup(ts => ts.GenerateToken(It.IsAny<User>()))
//				.Returns(expectedToken);

//			var result = await _userService.Authenticate(loginUser);

//			Assert.IsTrue(result.IsSuccess);
//			Assert.AreEqual(expectedToken, result.data);
//		}
//	}
//}
