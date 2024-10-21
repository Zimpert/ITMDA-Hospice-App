import pymysql

db = pymysql.connect(host="caringdb.c1auycogyth5.us-east-1.rds.amazonaws.com",
                     port=3306,
                     user="admin",
                     password="gkQzyBM*w^7p1B4E",
                     db="DB_Testing")
cursor = db.cursor()
cursor.execute("SELECT * FROM TB_Testing")
print(cursor.fetchall())
