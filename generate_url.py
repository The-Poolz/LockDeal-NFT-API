import os
import glob
import plantuml

def generate_urls(directory, output_file):
    # Getting PlantUML server URL from environment variable or using default value
    puml_server_url = os.getenv('PUML_SERVER_URL', 'https://www.plantuml.com/plantuml/svg/')
    pl = plantuml.PlantUML(url=puml_server_url)

    markdown_content = "# Diagrams\n\n<details><summary>Diagrams summary</summary>\n\n"
    markdown_content += "|**URL**|**PATH**|\n|:---|---:|\n"

    # Search all .puml files in a directory and subdirectories
    for file_path in glob.glob(os.path.join(directory, '**/*.puml'), recursive=True):
        with open(file_path, 'r') as file:
            diagram = file.read()

        url = pl.get_url(diagram)
        markdown_content += f"|[{url}]({url})|{file_path}|\n"
        
    markdown_content += "\n</details>\n"

    # Write all links to the output file
    with open(output_file, 'w') as f:
        f.write(markdown_content)

if __name__ == "__main__":
    # Getting directory path from environment variable or using default value
    directory = os.getenv('FILE_PATH', '.')
    output_file = 'url.txt'
    generate_urls(directory, output_file)
