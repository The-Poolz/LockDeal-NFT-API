import os
import glob
import plantuml

def generate_urls(directory, output_file):
    # Getting PlantUML server URL from environment variable or using default value
    puml_server_url = os.getenv('PUML_SERVER_URL', 'https://www.plantuml.com/plantuml/svg/')
    pl = plantuml.PlantUML(url=puml_server_url)

    all_links = ""

    # Search all .puml files in a directory and subdirectories
    for file_path in glob.glob(os.path.join(directory, '**/*.puml'), recursive=True):
        with open(file_path, 'r') as file:
            diagram = file.read()

        url = pl.get_url(diagram)
        all_links += f"## Diagram in {file_path}\n\n[Link to diagram]({url})\n\n"

    # Write all links to the output file
    with open(output_file, 'w') as f:
        f.write(all_links)

if __name__ == "__main__":
    # Getting directory path from environment variable or using default value
    directory = os.getenv('FILE_PATH', '.')
    output_file = 'url.txt'
    generate_urls(directory, output_file)
