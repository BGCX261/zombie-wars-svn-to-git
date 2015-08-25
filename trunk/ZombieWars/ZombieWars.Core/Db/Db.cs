using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;
using System.IO;

namespace ZombieWars.Core.Db
{
    public class DB
    {
        #region Поля

        /// <summary>
        /// Имя файла базы данных
        /// </summary>
        public static string dbFileName
        {
            get { return "Data.db"; }
        }

        /// <summary>
        /// Имя файла автоматически создаваемой резервной копии
        /// </summary>
        public static string dbAutoBackupFileName
        {
            get { return "Data.db.backup"; }
        }

        /// <summary>
        /// Имя временного файла автоматически создаваемой резервной копии
        /// </summary>
        private static string dbTemporaryAutoBackupFileName
        {
            get { return "Data._db"; }
        }

        /// <summary>
        /// База данных
        /// </summary>
        private static IObjectContainer db
        {
            get
            {
                if (_db != null) return _db;
                else
                {
                    if (!Open())
                    {
                        _db = null;
                    }
                    return _db;
                }
            }
        }
        private static IObjectContainer _db = null;

        /// <summary>
        /// Конфигурация программы
        /// </summary>
        public static Config Config
        {
            get
            {
                if (config == null)
                {
                    if (db != null)
                    {

                        IList<Config> configs = db.Query<Config>();
                        if (configs.Count == 0)
                        {
                            config = new Config();
                            db.Store(config);
                        }
                        else
                        {
                            config = configs[0];
                        }
                    }
                }
                return config;
            }
        }
        private static Config config = null;

        #endregion Поля

        #region Методы работы с БД

        /// <summary>
        /// Проверка доступности базы данных перед выполнением действия
        /// </summary>
        private static void testAndThrow()
        {
            if (db == null) throw new Exception("Не доступна база данных");
        }

        /// <summary>
        /// Получить идентификатор объекта
        /// </summary>
        /// <param name="Object">Объект</param>
        /// <returns>Идентификатор объекта</returns>
        public static long GetId(Object Object)
        {
            testAndThrow();
            return db.Ext().GetID(Object);
        }

        /// <summary>
        /// Запись объекта в БД
        /// </summary>
        /// <param name="Object">Объект</param>
        public static void Store(Object Object)
        {
            testAndThrow();
            db.Store(Object);
        }

        /// <summary>
        /// Удаление объекта из базы данных
        /// </summary>
        /// <param name="Object">Объект</param>
        public static void Delete(Object Object)
        {
            testAndThrow();
            db.Delete(Object);
        }

        /// <summary>
        /// Запрос всех элементов данного типа по предикату
        /// </summary>
        /// <typeparam name="Extent">Бизнес-класс</typeparam>
        /// <param name="Match">Предикат</param>
        /// <returns>Результат</returns>
        public static List<Extent> Query<Extent>(Predicate<Extent> Match)
        {
            testAndThrow();
            List<Extent> result = new List<Extent>(db.Query<Extent>(Match));
            return result;
        }

        /// <summary>
        /// Запрос всех элементов данного типа по предикату
        /// </summary>
        /// <typeparam name="Extent">Бизнес-класс</typeparam>
        /// <param name="Match">Предикат</param>
        /// <param name="Comparision">Сравнение</param>
        /// <returns>Результат</returns>
        public static List<Extent> Query<Extent>(Predicate<Extent> Match, System.Comparison<Extent> Comparision)
        {
            testAndThrow();
            List<Extent> result = new List<Extent>(db.Query<Extent>(Match));
            result.Sort(Comparision);
            return result;
        }


        /// <summary>
        /// Запрос всех элементов данного типа по предикату
        /// </summary>
        /// <typeparam name="Extent">Бизнес-класс</typeparam>
        /// <param name="Match">Предикат</param>
        /// <param name="Comparer">Сравнение</param>
        /// <returns>Результат</returns>
        public static List<Extent> Query<Extent>(Predicate<Extent> Match, IComparer<Extent> Comparer)
        {
            testAndThrow();
            List<Extent> result = new List<Extent>(db.Query<Extent>(Match));
            result.Sort(Comparer);
            return result;
        }


        /// <summary>
        /// Запрос всех элементов данного типа
        /// </summary>
        /// <typeparam name="Extent">Бизнес-класс</typeparam>
        /// <returns>Результат</returns>
        public static List<Extent> Query<Extent>()
        {
            testAndThrow();
            List<Extent> result = new List<Extent>(db.Query<Extent>());
            return result;
        }

        /// <summary>
        /// Запрос всех элементов данного типа
        /// </summary>
        /// <typeparam name="Extent">Бизнес-класс</typeparam>
        /// <param name="Comparer">Сравнение</param>
        /// <returns>Результат</returns>
        public static List<Extent> Query<Extent>(IComparer<Extent> Comparer)
        {
            testAndThrow();
            List<Extent> result = new List<Extent>(db.Query<Extent>());
            result.Sort(Comparer);
            return result;
        }

        /// <summary>
        /// Откат всех изменений
        /// </summary>
        public static void Rollback()
        {
            testAndThrow();
            db.Rollback();
            if (!Open())
            {
                throw new Exception("Ошибка при открытии базы данных");
            }
        }

        /// <summary>
        /// Сохранение всех изменений
        /// </summary>
        public static void Commit()
        {
            testAndThrow();
            db.Commit();
        }


        #endregion Методы работы с БД

        #region Базовые методы работы с БД

        /// <summary>
        /// Открытие БД
        /// </summary>
        /// <returns>Получилось ли открыть БД</returns>
        private static bool Open()
        {
            if (_db != null) Close();
            bool result = true;
            try
            {
                //Db4objects.Db4o.Config.IConfiguration config = Db4objects.Db4o.Db4oFactory.NewConfiguration();
                //config.ObjectClass(typeof(Operation)).ObjectField("date").Indexed(true);
                _db = Db4oFactory.OpenFile(dbFileName);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Проверка работоспособности БД
        /// </summary>
        /// <returns>Работоспособна ли БД</returns>
        public static bool Test()
        {
            return (db != null);
        }

        // Корректное закрытие БД
        public static void Close()
        {
            config = null;
            if (_db != null)
            {
                _db.Close();
            }
            _db = null;
        }

        /// <summary>
        /// Создание резервной копии
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        /// <returns>Удалось ли создать резервную копию</returns>
        public static bool Backup(string FileName)
        {
            testAndThrow();
            try
            {
                db.Ext().Backup(FileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Восстановление базы данных из резервной копии
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        /// <returns>Удалось ли восстановить базу данных</returns>
        public static bool Restore(string FileName)
        {
            testAndThrow();
            try
            {
                if (!Backup(dbTemporaryAutoBackupFileName)) return false;
                Close();
                try
                {
                    File.Copy(FileName, dbFileName, true);
                    if (!Open()) throw new Exception();
                    File.Copy(dbTemporaryAutoBackupFileName, dbAutoBackupFileName, true);
                    File.Delete(dbTemporaryAutoBackupFileName);
                }
                catch
                {
                    File.Copy(dbTemporaryAutoBackupFileName, dbFileName);
                    File.Delete(dbTemporaryAutoBackupFileName);
                    Open();
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Базовые методы работы с БД
    }
}
