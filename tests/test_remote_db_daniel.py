import pymysql

db = pymysql.connect(
                     #host="41.71.104.172",
                     host="trueideonline.co.za",
                     port=3306,
                     user="tru645",
                     password="Ve02c%c[2?;a",
                     db="tru645_CareApp")
cursor = db.cursor()
if True:
    cursor.execute("CREATE TABLE TB_Testing (PK_iID INT NOT NULL, sValue VARCHAR(255) NULL, CONTRAINT Testing_PK PRIMARY KEY (PK_iID))")
    cursor.execute("INSERT INTO TB_Testing VALUES (69, 'Noice')")
    cursor.commit()
cursor.execute("SELECT * FROM TB_Testing")
print(cursor.fetchall())
