//using Microsoft.EntityFrameworkCore;
//using Moq;


//namespace backnc.Tests
//{
//	public static class DbSetMockExtensions
//	{
//		public static Mock<DbSet<T>> CreateDbSetMock<T>(this IEnumerable<T> sourceList) where T : class
//		{
//			var queryable = sourceList.AsQueryable();
//			var dbSetMock = new Mock<DbSet<T>>();

//			dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
//			dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
//			dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
//			dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

//			return dbSetMock;
//		}

//		public static void ReturnsDbSet<T>(this Mock<DbSet<T>> dbSetMock, IEnumerable<T> sourceList) where T : class
//		{
//			dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(sourceList.AsQueryable().Provider);
//			dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(sourceList.AsQueryable().Expression);
//			dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(sourceList.AsQueryable().ElementType);
//			dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(sourceList.AsQueryable().GetEnumerator());
//		}
//	}
//}
