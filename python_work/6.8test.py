wangwang = {'类型': '狗',
            '主人': '小王'}
miaomiao = {'类型': '猫',
            '主人': '小李'}

pets = [wangwang, miaomiao]
for pet in pets:
    print("wangwang的类型是" + pet['类型'] + ",它的主人是" + pet['主人'])