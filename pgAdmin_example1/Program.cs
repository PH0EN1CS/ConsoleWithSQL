using Npgsql;
namespace pgAdmin_example1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sqlQuery;
            string con = "Host = localhost; Port = 5432;Database = practic1; Username = postgres; Password = 1";
            using (var conn = new NpgsqlConnection(con))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Выберите операцию(в ответе только цифра!\n1) Зарегистрироваться\n2) Удалить запись из базы данных\n3)Сделать заказ");
                    if (int.TryParse(Console.ReadLine(), out int answer))
                    {
                        switch (answer)
                        {
                            case 1:
                                string name, address;

                                Console.WriteLine("Ваше имя: ");
                                name = Console.ReadLine();
                                Console.WriteLine("Ваш Адресс: ");
                                address = Console.ReadLine();
                                Console.WriteLine("Ваш номер телефона: ");
                                if (long.TryParse(Console.ReadLine(), out long phone_number))
                                {
                                    sqlQuery = "INSERT INTO users (name, phone_number, address) VALUES(@name, @phone_number, @address)";
                                    using (var sql = new NpgsqlCommand(sqlQuery, conn))
                                    {
                                        sql.Parameters.AddWithValue("name", name);
                                        sql.Parameters.AddWithValue("address", address);
                                        sql.Parameters.AddWithValue("phone_number", phone_number);
                                        sql.ExecuteNonQuery();
                                        Console.WriteLine("Пользователь успешно зарегистрирован");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Введенный номер телефона неверный");
                                }
                                break;
                            case 2:
                                Console.WriteLine("Введите имя пользователя, которое хотите удалить");
                                name = Console.ReadLine();
                                sqlQuery = "DELETE FROM users WHERE name = @name";
                                using(var sql =  new NpgsqlCommand(sqlQuery, conn))
                                {
                                    sql.Parameters.AddWithValue("name", name);
                                    sql.ExecuteNonQuery();
                                    Console.WriteLine("Запись успешно удалена");
                                }
                                break;

                            case 3:
                                Console.WriteLine("Список товаров: ");
                                sqlQuery = "SELECT * FROM products";

                                using (var sql = new NpgsqlCommand(sqlQuery,conn))
                                {
                                    using (var reader = sql.ExecuteReader())
                                    {
                                        while(reader.Read())
                                        {
                                            int id = reader.GetInt32(reader.GetOrdinal(("product_id")));
                                            name  = reader.GetString(reader.GetOrdinal(("name")));
                                            decimal price = reader.GetDecimal(reader.GetOrdinal(("price")));
                                            Console.WriteLine($"id: {id} name: {name} price: {price}");

                                        }
                                    }
                                }
                                    break;
                            default:
                                Console.WriteLine("Введенные данные не верны");
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }


        }
    }
}

