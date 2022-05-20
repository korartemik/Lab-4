# Lab-4
## Генетический алгоритм
### Пункт 1
Реализация алгоритма:

•	Ген - вектор перемещения(сила и угол)

•	Хромосома - Координата в которой оказалась точка в момент времени t

•	Генотип - траектория движения точки

•	Особи - объект с траекторией и посчитанным значением фитнесс функции

•	Популяция - это лист особей на данной итерации

Создание начальной популяции:

Берём два рандомных зачения одно из которых будет сила, приложенная к точки (меньше fMax), второе угол(от 0° до 90°). Таким образом задаём траекторию движения "особи".

Скрещивание:

Берём два особи и создайм новую траектори по принципу ((x1+x2)/2, (y1+y2)/2), где x1,y1 и x2,y2 это координаты точки в момент времени t

Фитнес функция, состоит из 3 перемноженных коэффициентов :

1) Отношение разности Расстояния от точки где оканчивается траектория особи до точки (1,1) и расстояния от (0,0) до (1,1) к расстоянию от (0,0) до (1,1)
2) fine(от 0 до 1) в степени равному кол-ву раз когда "особь" попала на барьер
3) 0.1^n, где n - это кол-во точек вышедших за границу квадрата (0,0) - (1,1)

### Пункт 2

Тестирование 1 иттерации

![Рисунок1](https://user-images.githubusercontent.com/54327287/169373928-d1adbd7d-c8c8-4088-9b70-4800389578cf.png)

Как можно заметить больше всего времени тратиться на подсчёт фитнесс функции, а также не такое большое, но выделяющееся от остальных время на сортировку особей по убыванию значения фитнес функции

![image](https://user-images.githubusercontent.com/54327287/169374639-b2dc981e-079a-4c83-88cd-998ca159b2de.png)

Память тратится на хранение информации об особях

Тестирование 1000 иттераций

![Новый точечный рисунок](https://user-images.githubusercontent.com/54327287/169376121-9212516f-d783-4a8a-957e-696a305f6146.jpg)

Как можно заметить много времени тратиться также на подсчёт фитнесс функции, но ещё и на добавление элемента в лист(на изменения размера листа)

![Новый точечный рисунок](https://user-images.githubusercontent.com/54327287/169376897-2439a885-4de7-4db1-b686-50bdc7ff6ffc.jpg)

Тратиться много памяти при скрещивание при создание нового листа поинтов для новой особи, а также при соритировке особей

Возможные фиксы по скорости это создание массива сразу же нужного размера под новую особь(Ресайз коллекции)
По памяти вместо класса -> структура.

### Пункт 3 (Ресайз коллекции)

![image](https://user-images.githubusercontent.com/54327287/169385001-46943463-f458-4e0f-934d-9bed99a9cb93.png)

как можем заметить время на добавление уменьшилось в несколько раз так как.

![image](https://user-images.githubusercontent.com/54327287/169385370-0060783f-03c5-419d-87ea-a3dfe8996dd0.png)

Память также уменьшилась на 10Мб

### Пункт 4 

Замена класса на структуру

![image](https://user-images.githubusercontent.com/54327287/169386920-84024728-2a19-4c02-a463-0f58b48ab68d.png)

по времени произошло небольшое изменение в лучшую сторону

![image](https://user-images.githubusercontent.com/54327287/169387422-62fb8db3-4e33-4f11-983c-495918516a75.png)

По памяти общее значение осталось тем же, но по функции MergeIndivid можно заметить уменьшение затрачиваемой памяти

