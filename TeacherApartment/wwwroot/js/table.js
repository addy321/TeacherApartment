//生成table
function GenerateTable(JsonListDate,ListTitle,ListFeatures,Listmatch){
	var thead=""
	for (index in ListTitle) {
		thead+='<th>'+ListTitle[index]+'</th>'
	}
	$("thead>tr").html(thead)
	var tbody=""
	if (JsonListDate != null) {
		JsonListDate.forEach((data)=>{
			tbody+='<tr>'
			var id="";
			for(var datakey in data)
			{
				if (Listmatch != null) {
					var ismatch = true;
					for (var matchkey in Listmatch) {
						if (matchkey == datakey) {
							tbody += '<td>' + Listmatch[matchkey][data[datakey]] + '</td>'
							ismatch = false
						}
					}
					if (ismatch)
						tbody += '<td>' + data[datakey] + '</td>'
				} else {
					tbody += '<td>' + data[datakey] + '</td>'
                }
				
			}
			if (ListFeatures != null) {
				var Features = "<td>"
				for (var featureobj in ListFeatures) {
					for (var obj in ListFeatures[featureobj]) {
						Features += '<a href="#" onclick=' + obj + '(' + data["id"] + ')>' + ListFeatures[featureobj][obj] + '</a>'
					}
				}
				Features += '</td>'
				tbody += Features
            }
			
		})
	}else{
		tbody+="<tr><td>暂无数据</td></tr>"
	}
	$("tbody").html(tbody);
	
}

