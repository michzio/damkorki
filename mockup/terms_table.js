// JavaScript Document

var terms_table=
[[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[1,0,1,1,0,2,1],
[0,0,0,0,1,0,1],
[0,2,1,1,0,1,0],
[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0],,
[0,0,0,0,0,0,0],
[0,0,0,0,0,0,0]];

window.onload=
function() {alert ("Skrypt zaladowany podczas wczytania strony");}

init_terms_table(); 
/* funkcja inicjalizujaca tabele terminów
   koloruje tabele, ustawia na odpowiedni region
  */ 
function init_terms_table() { 
	fill_terms_table(); //wypelnia kolorami
	select_terms_table_region(); // ustawia odpowiedni obszar tabeli do wyświetlenia
}

function fill_terms_table() { 

	for(i = 0; i < terms_table.length; i++) { 
		for(j = 0; j < terms_table[i].length; j++) { 
			
			switch(terms_table[i][j]) { 
				case 0: 
					fill_unavailable_cell(i,j); 
					break; 
				case 1: 
					fill_reserved_cell(i,j); 
					break; 
				case 2: 
					fill_available_cell(i,j); 
					break;
				default: 
					fill_unavailable_cell(i,j); 
			}	
		}
	}
}

function fill_unavailable_cell(i,j) { 
		var color = "#f5f5f5"; 
		fill_cell(i, j, color); 
}

function fill_reserved_cell(i, j) { 
	var color = "#0000FF"; 
	fill_cell(i, j, color); 
}

function fill_available_cell(i, j) { 
	var color = "#ffffff"; 
	fill_cell(i, j, color); 
}

function fill_cell(i, j, color) { 
	
	var terms_table = document.getElementById("terms-table"); //pobieram elemtnt tabelki w HTML 
	var tableTDs = terms_table.getElementsByTagName("td");  // wszystkie komórki tabeli? 
	// i - to numer wierszu 
	// j - to numer kolumny 
	// idx obliczamy który to będzie numer
	// np 5 wiersz 5 kolumna to będzie 
	/* 
		  0 1 2 3 4 5 6 
		0 
		1
		2
		3
		4         x
		5         
		6
	*/ 
	var idx = i*7 + j -1; // nie wiem czy dobrze policzyłem ale musisz najlepiej potestowac chyba trzeba odjac -1 bo tableTDs tez sie od 0 zaczyna 
	var tdToFill = tableTDs[idx]; // pobieram ta komórke którą mam pomalować
	tdToFill.style.backgroundColor(color); 
}

// wybiera które 10 wierszy (5h) wyswietlic na ekranie
function select_terms_table_region() { 
	
	// 0 1 2 3 .... 47 
	// pasuje zsumowac ile jest opcji 0-9, 2-11, 4-13 , 6-15, 8-17, 10-19, 12-21, 14-23, 16-25, 18-27, 20-29, 22-31, 24-33, 26-35, 28-37, 30-39,
	// 32-41, 34-43, 36-45, 38-47  
    var regions_weight = array(20); // 20 regionów po 10 wierszy 5 h mozliwych 
	
	for(i = 0; i < regions_weight.length; i++) { 
			regions_weight[i] = 0; //inicjalizacja
			
		    // i = 0 => 0-9 sumujemy wiersze 
			// i = 1 => sumujemy 2 -11 wiersze 
			// i = 2 => sumujemy 4 -13 wiersze 
			// i = 3 => sumujemy 6- 15 wiersze 
			// i = 4 => to wiersze 8 - 17 
			// i = 19 = > to wiersze 38-47
			
			for( j = i*2; j < i*2 + 10; j++) { 
				regions_weight[i] += sum_array_elements(terms_table[j]); 
			}		
	}	
	
    var DEFAULT_REGION = 10;  // powinno byc jako globalna stała jak sie da 
	//sprawdz który z 20 regionów ma max wartosc (najwiecej wolnych terminów bo to sa 2 ewentualnie zajetych 1) 
	var max_region_idx =  DEFAULT_REGION; 
	var max_region_weight = 0; 
	for(i =0; i < regions_weight.length; i++) { 
		if(regions_weight[i] > max_region_weight) { 
			max_region_weight = regions_weight[i]; 
			max_region_idx = i; 
		}
	}
	
	// ustaw wyswietlone tylko wiersze tabeli dla danego regionu 
	// od i*2 - i*2 + 10 np 0 <= x < 10  lub 2 <= x < 12
	
	// pobierz wszystkie wiersze tablicy najpier zaincjalizuj jako ukryte
	// potem włacz te które maja byc wyswietlone ustwiajac 
	//
	// tableTRs[row_idx].style.display = "block";  oblicz sobie i*2 do < i*2 + 10  pozostałe maja byc ukryte 
	// raczej nie block tylko taki typ wyswietlania jak dla wierszy tabeli moze jest jakis table-row czy cos wydaje mi sie ze to nie jest block 
}
 
 function sum_array_elements(num_array) { 
 	
	var sum = 0; 
	for(i =0; i < num_array.length; i++) { 
		sum += num_array[i];
	}
	return sum; 
 }
