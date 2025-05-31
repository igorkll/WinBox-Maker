import pycdlib

def replace_file_in_udf_iso(iso_path, file_to_replace, new_file_path, output_iso_path):
    # Открываем ISO-образ
    iso = pycdlib.PyCdlib()
    iso.open(iso_path)


    iso.add_file(
        filename=new_file_path,  # Путь к новому файлу на вашем ПК
        udf_path=file_to_replace               # UDF путь (можно использовать тот же путь)
    )

    # Сохраняем изменения в новый ISO-образ
    iso.write(output_iso_path)
    iso.close()

# Пример использования
replace_file_in_udf_iso('winbox_temp/test.iso', '/install.wim', 'winbox_temp/install.wim', 'winbox_temp/test2.iso')