$(document)
	.ready(function () {
		$(function () {
			$('.dropdown-list')
				.slimScroll({
					color: '#3c6d97',
					railColor: '#cccccc',
					opacity: 1,
					right: "10px",
					borderRadius: 7,
					distance: '0',
					size: '7px',
					alwaysVisible: true,
					railVisible: true
				});
		});

		$(function () {
			var text = $(".select select").find("option[selected]").html();
			$(".select span").html(text);
		});

		$(".select ul li")
			.click(function () {
				var text = $(this).html();
				var index = $(this).index();
				$(".select span").html(text);
				$(".select select").children("option").removeAttr("selected");
				$(".select select").children("option").eq(index).attr("selected", "selected");
				$(".select ul").slideUp();
				$(".select").removeClass("active");
			});

		$(document.getElementById("Search"))
			.change(function () {
				$(this).parent("form").submit();
			});

		$(document.getElementById("Sexs"))
			.change(function () {
				$(this).parent("form").submit();
			});

		$(function () {
			$('*[data-href]')
				.click(function () {
					window.location = $(this).data('href');
					return false;
				});
		});

		$(".form")
			.on("input",
				".input-val",
				function () {
					var value = $(this).val();
					if (value != '') {
						$(this).parent().children(".clear-input").fadeIn(100);
					} else {
						$(this).parent().children(".clear-input").fadeOut(100);
					}
				});

		$(".clear-input")
			.click(function () {
				$(this).parent().children('input[type="text"]').val("");
				$(this).parent().children('textarea').val("");
				$(this).fadeOut(100);
			});

		$(".check-radio")
			.change(function () {
				var name = $(this).closest("label input").attr("name");
				var list = $(this).parents(".coll-wrap").find("[for*='" + name + "']");
				list.each(function () {
					$(this).closest("label.radio-label-active").find("input:first").attr("checked", false);
					$(this).closest("label.radio-label-active").removeClass("radio-label-active");
				});

				$(this).closest("label").addClass("radio-label-active");
				$(this).closest("label.radio-label-active").find("input:first").attr("checked", true);
			});

		$(function () {
			$('.datepicker')
				.datetimepicker({
					locale: 'uk',
					format: 'DD-MM-YYYY'
				});
		});

		$(function () {
			$('.datetimepicker')
				.datetimepicker({
					locale: 'uk',
					format: 'DD-MM-YYYY HH:mm:ss'
				});
		});

		$(".select-wrap > .hint")
			.click(function () {
				$(this).toggleClass("select-wrap-active");
				$(this).parent().find(".popup-select").slideToggle();
			});

		$(".check-dropdown-multi-select")
			.change(function () {
				var keys = "";
				var values = "";
				var oldValue = $(this).closest(".select-wrap").children("div.display-n").html();
				var list;
				if ($(this).parent().hasClass("dropdown-label-active")) {
					$(this).closest("label.dropdown-label-active").removeClass("dropdown-label-active");

					$(this).parents(".dropdown-list-wrap").find("input:first").val("");
					$(this).closest(".select-wrap").children("div.hint").html("");

					list = $(this).closest("div.popup-select").find(".dropdown-list li label.dropdown-label-active");
					list.each(function () {
						var key = $(this).children("input:eq(0)").val();
						var value = $(this).children("input:eq(1)").val();
						if (keys == "") {
							keys = key;
							values = value;
						} else {
							keys = keys + "," + key;
							values = values + "|" + value;
						}
					});

					$(this).parents(".dropdown-list-wrap").find("input:first").val(keys);
					if (values != "") {
						$(this).closest(".select-wrap").children("div.hint").html(values);
					} else {
						$(this).closest(".select-wrap").children("div.hint").html(oldValue);
					}
				} else {
					$(this).closest("label").addClass("dropdown-label-active");

					$(this).parents(".dropdown-list-wrap").find("input:first").val("");
					$(this).closest(".select-wrap").children("div.hint").html("");

					list = $(this).closest("div.popup-select").find(".dropdown-list li label.dropdown-label-active");
					list.each(function () {
						var key = $(this).children("input:eq(0)").val();
						var value = $(this).children("input:eq(1)").val();
						if (keys == "") {
							keys = key;
							values = value;
						} else {
							keys = keys + "," + key;
							values = values + "|" + value;
						}
					});

					$(this).parents(".dropdown-list-wrap").find("input:first").val(keys);
					if (values != "") {
						$(this).closest(".select-wrap").children("div.hint").html(values);
					} else {
						$(this).closest(".select-wrap").children("div.hint").html(oldValue);
					}
				}
			});

		function initCheckBoxSingle(checkBoxItem) {
			$(checkBoxItem).change(function () {
				if ($(checkBoxItem).parent().hasClass("dropdown-label-active")) {
					$(checkBoxItem).closest("label.dropdown-label-active").removeClass("dropdown-label-active");
					$(checkBoxItem).parents(".dropdown-list-wrap").find("input:first").val("");
					var oldValue = $(checkBoxItem).closest(".select-wrap").children("div.display-n").html();
					$(checkBoxItem).closest(".select-wrap").children("div.hint").html(oldValue);
				} else {
					var list = $(checkBoxItem).closest("div.popup-select").find(".dropdown-list li label");
					list.each(function () {
						$(this).closest("label.dropdown-label-active").removeClass("dropdown-label-active");
					});
					$(checkBoxItem).closest("label").addClass("dropdown-label-active");

					var key = $(checkBoxItem).siblings("input:first").val();
					var value = $(checkBoxItem).siblings("input:eq(1)").val();
					$(checkBoxItem).parents(".dropdown-list-wrap").find("input:first").val(key);
					$(checkBoxItem).closest(".select-wrap").children("div.hint").html(value);
				}
			});
		};

		$(".check-dropdown-single-select")
			.each(function (index, value) {
				initCheckBoxSingle($(value));
			});

		$(".form")
			.on("input",
				".select-dropdownitems",
				function () {
					var value = $(this).val();
					var list = $(this).closest("div.popup-select").find(".dropdown-list li label span");
					if (value != "") {
						list.each(function () {
							var text = $(this).text();
							if (text.toUpperCase().indexOf(value.toUpperCase()) !== -1) { // as like in sql
								$(this).closest("li").show();
							} else {
								$(this).closest("li").hide();
							}
						});
					} else {
						list.each(function () {
							$(this).closest("li").show();
						});
					}
				});

		$(".glyphicon-eye-open")
			.click(function () {
				if ($(this).hasClass("glyphicon-eye-open")) {
					$(this).siblings(".form-control").attr('type', 'text');
					$(this).addClass("glyphicon-eye-close");
					$(this).removeClass("glyphicon-eye-open");
				} else {
					$(this).siblings(".form-control").attr('type', 'password');
					$(this).addClass("glyphicon-eye-open");
					$(this).removeClass("glyphicon-eye-close");
				}
			});

		//Vertical tabs
		$("div.bhoechie-tab-menu>div.list-group>a")
			.click(function (e) {
				e.preventDefault();
				$(this).siblings('a.active').removeClass("active");
				$(this).addClass("active");
				var index = $(this).index();
				$("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");
				$("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");
			});

		//Dragable
		$(function () {
			//do not change file ../Scripts/jquery.sortable.js
			$(".source, .target")
				.sortable({
					connectWith: ".connected"
				})
				.bind("sortupdate",
					function () {
						var items = [];
						$("ul.target")
							.children()
							.each(function () {
								items.push($(this).children("input:eq(0)").val());
							});
						$(this).parents(".form-group").find("input:first").val(items.join());
					});
		});

		//Results
		$.fn.extend({
			treeview: function () {
				return this.each(function () {
					// Initialize the top levels;
					var tree = $(this);

					tree.addClass('treeview-tree');
					tree.find('li')
						.each(function () {
							var stick = $(this);
						});
					tree.find('li')
						.has("ul")
						.each(function () {
							var branch = $(this); //li with children ul

							branch.prepend("<i class='tree-indicator glyphicon glyphicon-chevron-right'></i>");
							branch.addClass('tree-branch');
							branch.on('click',
								function (e) {
									if (this == e.target) {
										var icon = $(this).children('i:first');

										icon.toggleClass("glyphicon-chevron-down glyphicon-chevron-right");
										$(this).children().children().toggle();
									}
								});
							branch.children().children().toggle();

							/**
							 *	The following snippet of code enables the treeview to
							 *	function when a button, indicator or anchor is clicked.
							 *
							 *	It also prevents the default function of an anchor and
							 *	a button from firing.
							 */
							branch.children('.tree-indicator, button, a')
								.click(function (e) {
									branch.click();

									e.preventDefault();
								});
						});
				});
			}
		});

		/**
		 *	The following snippet of code automatically converst
		 *	any '.treeview' DOM elements into a treeview component.
		 */
		$(window)
			.on('load',
				function () {
					$('.treeview')
						.each(function () {
							var tree = $(this);
							tree.treeview();
						});
				});

		$.fn.gMapsLatLonPicker = (function (options) {
			var _self = this;

			_self.params = {
				defLat: options.defLat ? options.defLat : 49.0139,
				defLng: options.defLng ? options.defLng : 31.2858,
				defZoom: options.defZoom ? options.defZoom : 6,
				queryLocationNameWhenLatLngChanges: true,
				queryElevationWhenLatLngChanges: true,
				mapOptions: {
					mapTypeId: google.maps.MapTypeId.ROADMAP,
					mapTypeControl: false,
					disableDoubleClickZoom: true,
					zoomControlOptions: true,
					streetViewControl: false
				},
				strings: {
					markerText: "Пересувайте цей маркер",
					error_empty_field: "Неможливо знайти координати для цього місця",
					error_no_results: "Неможливо знайти координати для цього місця"
				},
				displayError: function (message) {
					alert(message);
				}
			};

			_self.vars = {
				ID: null,
				LATLNG: null,
				map: null,
				marker: null,
				geocoder: null
			};

			var setPosition = function (position) {
				_self.vars.marker.setPosition(position);
				_self.vars.map.panTo(position);

				$(_self.vars.cssID + ".gllpZoom").val(_self.vars.map.getZoom());
				$(_self.vars.cssID + ".gllpLongitude").attr('value', position.lng());
				$(_self.vars.cssID + ".gllpLongitude").val(position.lng());
				$(_self.vars.cssID + ".gllpLatitude").attr('value', position.lat());
				$(_self.vars.cssID + ".gllpLatitude").val(position.lat());

				$(_self.vars.cssID).trigger("location_changed", $(_self.vars.cssID));

				if (_self.params.queryLocationNameWhenLatLngChanges) {
					getLocationName(position);
				}
				if (_self.params.queryElevationWhenLatLngChanges) {
					getElevation(position);
				}
			};

			// for reverse geocoding
			var getLocationName = function (position) {
				var latlng = new google.maps.LatLng(position.lat(), position.lng());
				_self.vars.geocoder.geocode({ 'latLng': latlng },
					function (results, status) {
						if (status == google.maps.GeocoderStatus.OK && results[0]) {
							$(_self.vars.cssID + ".gllpLocationName").attr('value', results[1].formatted_address);
							$(_self.vars.cssID + ".gllpLocationName").val(results[1].formatted_address);
							//update search field with formatted address
							$(_self.vars.cssID + ".gllpSearch").attr('value', results[1].formatted_address);
							$(_self.vars.cssID + ".gllpSearch").val(results[1].formatted_address);

							var address_components = results[0].address_components;
							var components = {};
							jQuery.each(address_components,
								function (k, v1) { jQuery.each(v1.types, function (k2, v2) { components[v2] = v1.long_name }); })

							$(_self.vars.cssID + ".gllpCity").attr('value', components.locality);
							$(_self.vars.cssID + ".gllpCity").val(components.locality);
							$(_self.vars.cssID + ".gllpPostalCode").attr('value', components.postal_code);
							$(_self.vars.cssID + ".gllpPostalCode").val(components.postal_code);
							$(_self.vars.cssID + ".gllpStreet").attr('value', components.route);
							$(_self.vars.cssID + ".gllpStreet").val(components.route);
							$(_self.vars.cssID + ".gllpNumber").attr('value', components.street_number);
							$(_self.vars.cssID + ".gllpNumber").val(components.street_number);
						} else {
							$(_self.vars.cssID + ".gllpLocationName").attr('value', "");
							$(_self.vars.cssID + ".gllpLocationName").val("");
						}
						$(_self.vars.cssID).trigger("location_name_changed", $(_self.vars.cssID));
					});
			};

			// for getting the elevation value for a position
			var getElevation = function (position) {
				var latlng = new google.maps.LatLng(position.lat(), position.lng());

				var locations = [latlng];

				var positionalRequest = { 'locations': locations };

				_self.vars.elevator.getElevationForLocations(positionalRequest,
					function (results, status) {
						if (status == google.maps.ElevationStatus.OK) {
							if (results[0]) {
								$(_self.vars.cssID + ".gllpElevation").val(results[0].elevation.toFixed(3));
							} else {
								$(_self.vars.cssID + ".gllpElevation").val("");
							}
						} else {
							$(_self.vars.cssID + ".gllpElevation").val("");
						}
						$(_self.vars.cssID).trigger("elevation_changed", $(_self.vars.cssID));
					});
			};

			// search function
			var performSearch = function (string, silent) {
				if (string == "") {
					if (!silent) {
						_self.params.displayError(_self.params.strings.error_empty_field);
					}
					return;
				}
				_self.vars.geocoder.geocode(
					{ "address": string },
					function (results, status) {
						if (status == google.maps.GeocoderStatus.OK) {
							$(_self.vars.cssID + ".gllpZoom").val(12);
							_self.vars.map.setZoom(parseInt($(_self.vars.cssID + ".gllpZoom").val()));
							setPosition(results[0].geometry.location);
						} else {
							if (!silent) {
								_self.params.displayError(_self.params.strings.error_no_results);
							}
						}
					}
				);
			};

			var publicfunc = {
				init: function (object) {

					if (!$(object).attr("id")) {
						if ($(object).attr("name")) {
							$(object).attr("id", $(object).attr("name"));
						} else {
							$(object).attr("id", "_MAP_" + Math.ceil(Math.random() * 10000));
						}
					}

					_self.vars.ID = $(object).attr("id");
					_self.vars.cssID = "#" + _self.vars.ID + " ";

					_self.params.defLat = parseFloat($(_self.vars.cssID + ".gllpLatitude").val().replace(",", "."))
						? parseFloat($(_self.vars.cssID + ".gllpLatitude").val().replace(",", "."))
						: _self.params.defLat;
					_self.params.defLng = parseFloat($(_self.vars.cssID + ".gllpLongitude").val().replace(",", "."))
						? parseFloat($(_self.vars.cssID + ".gllpLongitude").val().replace(",", "."))
						: _self.params.defLng;
					_self.params.defZoom = $(_self.vars.cssID + ".gllpZoom").val()
						? parseInt($(_self.vars.cssID + ".gllpZoom").val())
						: _self.params.defZoom;


					_self.vars.LATLNG = new google.maps.LatLng(_self.params.defLat, _self.params.defLng);

					_self.vars.MAPOPTIONS = _self.params.mapOptions;
					_self.vars.MAPOPTIONS.zoom = _self.params.defZoom;
					_self.vars.MAPOPTIONS.center = _self.vars.LATLNG;

					_self.vars.map = new google.maps.Map($(_self.vars.cssID + ".gllpMap").get(0), _self.vars.MAPOPTIONS);
					_self.vars.geocoder = new google.maps.Geocoder();
					_self.vars.elevator = new google.maps.ElevationService();

					_self.vars.marker = new google.maps.Marker({
						position: _self.vars.LATLNG,
						map: _self.vars.map,
						title: _self.params.strings.markerText,
						draggable: true
					});

					// Set position on doubleclick
					google.maps.event.addListener(_self.vars.map,
						'dblclick',
						function (event) {
							setPosition(event.latLng);
						});

					// Set position on marker move
					google.maps.event.addListener(_self.vars.marker,
						'dragend',
						function (event) {
							setPosition(_self.vars.marker.position);
						});

					// Set zoom feld's value when user changes zoom on the map
					google.maps.event.addListener(_self.vars.map,
						'zoom_changed',
						function (event) {
							$(_self.vars.cssID + ".gllpZoom").val(_self.vars.map.getZoom());
							$(_self.vars.cssID).trigger("location_changed", $(_self.vars.cssID));
						});

					// Update location and zoom values based on input field's value
					$(_self.vars.cssID + ".gllpSearchCoordinatesAddressButton")
						.bind("click",
							function () {
								var lat = $(_self.vars.cssID + ".gllpLatitude").val();
								var lng = $(_self.vars.cssID + ".gllpLongitude").val();
								var latlng = new google.maps.LatLng(lat, lng);
								_self.vars.map.setZoom(parseInt($(_self.vars.cssID + ".gllpZoom").val()));
								setPosition(latlng);
							});

					// Search by gllpSearchSeparateAddressButton
					$(_self.vars.cssID + ".gllpSearchSeparateAddressButton")
						.bind("click",
							function () {
								performSearch(
									$(_self.vars.cssID + ".gllpCity").val() +
									$(_self.vars.cssID + ".gllpStreet").val() +
									$(_self.vars.cssID + ".gllpNumber").val(),
									false);

								_self.params.defZoom = _self.vars.map.getZoom();
								_self.vars.map.setZoom(_self.params.defZoom);
							});

					// Search by gllpSearchFullAddressButton
					$(_self.vars.cssID + ".gllpSearchFullAddressButton")
						.bind("click",
							function () {
								performSearch($(_self.vars.cssID + ".gllpSearch").val(), false);

								_self.params.defZoom = _self.vars.map.getZoom();
								_self.vars.map.setZoom(_self.params.defZoom);
							});

					// Search function by gllp_perform_search listener
					$(document)
						.bind("gllp_perform_search",
							function (event, object) {
								performSearch($(object).attr('string'), true);
							});

					// Zoom function triggered by gllp_perform_zoom listener
					$(document)
						.bind("gllp_update_fields",
							function (event) {
								var lat = $(_self.vars.cssID + ".gllpLatitude").val();
								var lng = $(_self.vars.cssID + ".gllpLongitude").val();
								var latlng = new google.maps.LatLng(lat, lng);
								_self.vars.map.setZoom(parseInt($(_self.vars.cssID + ".gllpZoom").val()));
								setPosition(latlng);
							});
				},

				params: _self.params

			};

			return publicfunc;
		});

		$(document)
			.ready(function () {
				if (!$.gMapsLatLonPickerNoAutoInit) {
					$(".gllpLatlonPicker")
						.each(function () {
							$(document)
								.gMapsLatLonPicker(
									{
										defLat: parseFloat($(".gllpLatitude").val().replace(",", ".")),
										defLng: parseFloat($(".gllpLongitude").val().replace(",", ".")),
										defZoom: parseInt($(".gllpZoom").val())
									}
								)
								.init($(this));
						});
				}
			});

		$(".ul-search")
			.keyup(function () {
				var valThis = this.value.toLowerCase();
				$(this)
					.siblings("ul")
					.children("li")
					.each(function () {
						var text = $(this).children("label").text();
						if (valThis === '' || text.toLowerCase().indexOf(valThis) >= 0) {
							$(this).show();
						} else {
							$(this).hide();
						}
					});
			});

		$(".switch")
			.click(function () {
				if ($(this).children("input[type='checkbox']").is(":checked")) {
					$(this).children("input[type='checkbox']").prop("checked", false);
					$(this).siblings("span.input-group").children("input").prop("value", "");
					updateTabsWithDateTimeHidden(this);
					$(this).siblings("span.input-group").hide();
				} else {
					$(this).children("input[type='checkbox']").prop("checked", true);
					$(this).siblings("span.input-group").show();
				}
			});

		$(".datetimepicker")
			.on("dp.change",
				function () {
					updateTabsWithDateTimeHidden(this);
				});

		function updateTabsWithDateTimeHidden(obj) {
			var liItems = $(obj).parents(".bhoechie-tab").find("li");
			var arr = [];
			for (var j = 0; j < liItems.length; j++) {
				var item = liItems[j];
				if ($(item).find("input[type='checkbox']").is(":checked")) {
					var dateTimeStr = $(item).find("input[type='text']").val();
					if (dateTimeStr !== "") {
						var day = dateTimeStr.substring(0, 2);
						var month = dateTimeStr.substring(3, 5);
						var dateTimeStrL = dateTimeStr.substring(6, dateTimeStr.lenght);
						var dateTimeStrNew = month + "-" + day + "-" + dateTimeStrL;

						var d = new Date(dateTimeStrNew);
						arr.push(
						{
							"Id": $(item).find("input[type='hidden']").val(),
							"ReturnTime": "\/Date(" + d.getTime() + "-0000)\/"
						});
					}
				}
			}
			$(obj).parents(".form-group").children("input[type='hidden']:first").val(JSON.stringify(arr));
		}

		$('#race-add-1')
			.change(function () {
				var value = $(this).parents(".dropdown-list-wrap").children("input:first").val();

				$('#race-add-21').parents(".dropdown-list-wrap").children("input:first").val("");
				var oldValue21 = $('#race-add-21').parents(".select-wrap").children("div.display-n").html();
				$('#race-add-21').parents(".select-wrap").children("div.hint").html(oldValue21);

				$('#race-add-22').parents(".dropdown-list-wrap").children("input:first").val("");
				var oldValue22 = $('#race-add-21').parents(".select-wrap").children("div.display-n").html();
				$('#race-add-22').parents(".select-wrap").children("div.hint").html(oldValue22);
				if (value !== "") {
					var data = { id: value };
					$.ajax({
						url: "seasons",
						type: 'GET',
						data: data,
						success: function (items) {
							var block = $('#race-add-21');
							block.empty();
							var i = 0;
							$('#race-add-21').parents("div.form-group").show();
							items.forEach(item => {
								var li = createDropdownItem(item, "SeasonId[" + i++ + "]");
								li.appendTo(block);
								initCheckBoxSingle(li.find(".check-dropdown-single-select"));
							});
						}
					});
					$.ajax({
						url: "points",
						type: 'GET',
						data: data,
						success: function (items) {
							var block = $('#race-add-22');
							block.empty();
							var i = 0;
							$('#race-add-22').parents("div.form-group").show();
							items.forEach(item => {
								var li = createDropdownItem(item, "PointId[" + i++ + "]");
								li.appendTo(block);
								initCheckBoxSingle(li.find(".check-dropdown-single-select"));
							});
						}
					});
				} else {
					$('#race-add-21').parents("div.form-group").hide();
					$('#race-add-22').parents("div.form-group").hide();
				}
			});

		$('#organization-add-1')
			.change(function () {
				var value = $(this).parents(".dropdown-list-wrap").children("input:first").val();

				$('#organization-add-21').parents(".dropdown-list-wrap").children("input:first").val("");
				var oldValue21 = $('#organization-add-21').parents(".select-wrap").children("div.display-n").html();
				$('#organization-add-21').parents(".select-wrap").children("div.hint").html(oldValue21);

				if (value !== "") {
					var data = { organizationTypeId: value };
					$.ajax({
						url: "organizations",
						type: 'GET',
						data: data,
						success: function (items) {
							var block = $('#organization-add-21');
							block.empty();
							var i = 0;
							$('#organization-add-21').parents("div.form-group").show();
							items.forEach(item => {
								var li = createDropdownItem(item, "OrganizationId[" + i++ + "]");
								li.appendTo(block);
								initCheckBoxSingle(li.find(".check-dropdown-single-select"));
							});
						}
					});
				} else {
					$('#organization-add-21').parents("div.form-group").hide();
				}
			});

		function createDropdownItem(item, forName) {
			return $('<li/>')
				.append(
					$('<label/>',
					{
						for: forName,
						class: "dropdown-label"
					})
					.append(
						$('<i/>')
					)
					.append(
						$("<span/>",
						{
							text: item.Text
						})
					)
					.append(
						$("<input/>",
						{
							type: "hidden",
							value: item.Value,
							name: forName + ".Value"
						})
					)
					.append(
						$("<input/>",
						{
							type: "hidden",
							value: item.Text,
							name: forName + ".Text"
						})
					)
					.append(
						$("<input/>",
						{
							type: "checkbox",
							value: item.Selected,
							name: forName + ".Selected",
							class: "check-dropdown-single-select",
							id: forName
						})
					)
					.append(
						$("<input/>",
						{
							type: "hidden",
							value: item.Selected,
							name: forName + ".Selected"
						})
					)
				);
		}
	});
